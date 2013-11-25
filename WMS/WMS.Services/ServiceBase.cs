using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess;
using WMS.ServicesInterface;

namespace WMS.Services
{
    public class ServiceBase
    {
        protected T Transaction<T>(Func<TransactionContext, T> action)
        {
            using (var context = new SystemContext())
            {
                return context.TransactionSync(action);
            }
        }

        protected void Transaction(Action<TransactionContext> action)
        {
            using (var context = new SystemContext())
            {
                context.TransactionSync(action);
            }
        }
    }
}