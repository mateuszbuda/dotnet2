﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Client.Dialogs
{
    public class BaseDialog : Window
    {
        protected IWarehousesService WarehousesService { get; private set; }
        protected IPartnersService PartnersService { get; private set; }
        protected IProductsService ProductsService { get; private set; }

        public BaseDialog()
        {
            var warehouseChannelFactory = new ChannelFactory<IWarehousesService>("BasicHttpBinding_IWarehousesService");
            WarehousesService = warehouseChannelFactory.CreateChannel();

            var partnersChannelFactory = new ChannelFactory<IPartnersService>("BasicHttpBinding_IPartnersService");
            PartnersService = partnersChannelFactory.CreateChannel();

            var productsChannelFactory = new ChannelFactory<IProductsService>("BasicHttpBinding_IProductsService");
            ProductsService = productsChannelFactory.CreateChannel();
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

        private void DefaultExceptionHandler(Exception e)
        {
            MessageBox.Show("Wystąpił błąd podczas komunikacji z serwerem.\n\n" + e.Message + (e.InnerException == null ? "" : "\n\n" + e.InnerException.Message + (e.InnerException.InnerException == null ? "" : "\n\n" + e.InnerException.InnerException.Message + (e.InnerException.InnerException.InnerException == null ? "" : "\n\n" + e.InnerException.InnerException.InnerException.Message))), "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected void Execute(Action action, Action success = null, Action<Exception> exception = null)
        {
            Execute(() => { action(); return false; }, success != null ? (x => success()) : (Action<bool>)null, exception);
        }
    }
}