using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace WMS.DatabaseAccess
{
    public class SystemContext : IDisposable
    {
        private SystemEntities entities;
        private object syncObj;

        public SystemContext()
        {
            entities = new SystemEntities();
            syncObj = new object();
        }

        public void TransactionSync(Action<TransactionContext> action)
        {
            TransactionSync<bool>(tc => { action(tc); return false; });
        }

        public T TransactionSync<T>(Func<TransactionContext, T> action)
        {
            lock (syncObj)
            {
                using (var transaction = new TransactionScope())
                {
                    using (var tc = new TransactionContext(entities))
                    {
                        T r = action(tc);

                        if (!tc.Rollback)
                        {
                            entities.SaveChanges();
                            transaction.Complete();
                        }

                        return r;
                    }
                }
            }
        }

        public void Transaction(Action<TransactionContext> action, Action success = null, Action<Exception> exception = null)
        {
            Transaction<bool>(tc => { action(tc); return false; }, success != null ? x => success() : (Action<bool>)null, exception);
        }

        public void Transaction<T>(Func<TransactionContext, T> action, Action<T> success = null, Action<Exception> exception = null)
        {
            Task<T> task = new Task<T>(() => TransactionSync(action));

            if (success != null)
                task.ContinueWith(t => success(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

            if (exception != null)
                task.ContinueWith(t => exception(t.Exception), TaskContinuationOptions.OnlyOnFaulted);

            task.Start();
        }

        public void Dispose()
        {
        }
    }
}
