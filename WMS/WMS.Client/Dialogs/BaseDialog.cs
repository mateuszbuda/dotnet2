using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Client.Dialogs
{
    public class BaseDialog : Window
    {
        protected IWarehousesService WarehousesService { get; private set; }
        protected IPartnersService PartnersService { get; private set; }
        protected IProductsService ProductsService { get; private set; }
        protected IGroupsService GroupsService { get; set; }

        public BaseDialog(MainWindow mainWindow)
        {
            var warehouseChannelFactory = new ChannelFactory<IWarehousesService>("SecureBinding_IWarehousesService");
            warehouseChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
            warehouseChannelFactory.Credentials.UserName.Password = mainWindow.Password;
            WarehousesService = warehouseChannelFactory.CreateChannel();

            var partnersChannelFactory = new ChannelFactory<IPartnersService>("SecureBinding_IPartnersService");
            partnersChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
            partnersChannelFactory.Credentials.UserName.Password = mainWindow.Password;
            PartnersService = partnersChannelFactory.CreateChannel();

            var productsChannelFactory = new ChannelFactory<IProductsService>("SecureBinding_IProductsService");
            productsChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
            productsChannelFactory.Credentials.UserName.Password = mainWindow.Password;
            ProductsService = productsChannelFactory.CreateChannel();

            var groupsChannelFactory = new ChannelFactory<IGroupsService>("SecureBinding_IGroupsService");
            groupsChannelFactory.Credentials.UserName.UserName = mainWindow.Username;
            groupsChannelFactory.Credentials.UserName.Password = mainWindow.Password;
            GroupsService = groupsChannelFactory.CreateChannel();
        }

        protected void Execute<T>(Func<T> action, Action<T> success = null, Action<Exception> exception = null)
        {
            var ts = TaskScheduler.FromCurrentSynchronizationContext();
            var task = new Task<T>(action);

            if (success != null)
                task.ContinueWith(t => success(t.Result), new CancellationToken(), TaskContinuationOptions.OnlyOnRanToCompletion, ts);

            exception = exception != null ? exception : DefaultExceptionHandler;
            task.ContinueWith(t => exception(t.Exception), new CancellationToken(), TaskContinuationOptions.OnlyOnFaulted, ts);

            task.Start();
        }

        protected void DefaultExceptionHandler(Exception e)
        {
            if (e.InnerException != null && e.InnerException.GetType() == typeof(FaultException<ServiceException>))
                MessageBox.Show((e.InnerException as FaultException<ServiceException>).Detail.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Nieznany błąd wewnętrzny serwera.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            //MessageBox.Show("Wystąpił błąd podczas komunikacji z serwerem.\n\n" + e.Message + (e.InnerException == null ? "" : "\n\n" + e.InnerException.Message + (e.InnerException.InnerException == null ? "" : "\n\n" + e.InnerException.InnerException.Message + (e.InnerException.InnerException.InnerException == null ? "" : "\n\n" + e.InnerException.InnerException.InnerException.Message))), "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected void Execute(Action action, Action success = null, Action<Exception> exception = null)
        {
            Execute(() => { action(); return false; }, success != null ? (x => success()) : (Action<bool>)null, exception);
        }
    }
}
