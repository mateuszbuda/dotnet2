using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.DatabaseAccess
{
    public class TransactionContext : IDisposable
    {
        public SystemEntities Entities { get; private set; }
        public bool Rollback { get; set; }

        public TransactionContext(SystemEntities entities)
        {
            Rollback = false;
            Entities = entities;
        }

        public void Dispose()
        {
        }
    }
}
