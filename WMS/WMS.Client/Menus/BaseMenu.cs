﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Client.Menus
{
    public class BaseMenu : UserControl
    {
        protected IWarehousesService WarehousesService { get; private set; }
        protected IPartnersService PartnersService { get; private set; }
        protected IProductsService ProductsService { get; private set; }
        protected IGroupsService GroupService { get; private set; }

        public BaseMenu()
        {
            var warehouseChannelFactory = new ChannelFactory<IWarehousesService>("BasicHttpBinding_IWarehousesService");
            WarehousesService = warehouseChannelFactory.CreateChannel();

            var partnersChannelFactory = new ChannelFactory<IPartnersService>("BasicHttpBinding_IPartnersService");
            PartnersService = partnersChannelFactory.CreateChannel();

            var productsChannelFactory = new ChannelFactory<IProductsService>("BasicHttpBinding_IProductsService");
            ProductsService = productsChannelFactory.CreateChannel();

            var groupsChannelFactory = new ChannelFactory<IGroupsService>("BasicHttpBinding_IGroupsService");
            GroupService = groupsChannelFactory.CreateChannel();
        }

        public void Execute<T>(Func<T> action, Action<T> success = null, Action<Exception> exception = null)
        {
            var ts = TaskScheduler.FromCurrentSynchronizationContext();
            var task = new Task<T>(action);

            if (success != null)
                task.ContinueWith(t => success(t.Result), new CancellationToken(), TaskContinuationOptions.OnlyOnRanToCompletion, ts);

            exception = exception != null ? exception : DefaultExceptionHandler;
            task.ContinueWith(t => exception(t.Exception), new CancellationToken(), TaskContinuationOptions.OnlyOnFaulted, ts);

            task.Start();
        }

        private void DefaultExceptionHandler(Exception e)
        {
            MessageBox.Show("Wystąpił błąd podczas komunikacji z serwerem.\n\n" + e.Message + (e.InnerException == null ? "" : "\n\n" + e.InnerException.Message + (e.InnerException.InnerException == null ? "" : "\n\n" + e.InnerException.InnerException)), "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Execute(Action action, Action success = null, Action<Exception> exception = null)
        {
            Execute(() => { action(); return false; }, success != null ? (x => success()) : (Action<bool>)null, exception);
        }
    }
}
