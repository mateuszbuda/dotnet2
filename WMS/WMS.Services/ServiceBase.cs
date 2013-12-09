using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess;
using WMS.Services.Assemblers;
using WMS.ServicesInterface;

namespace WMS.Services
{
    public class ServiceBase
    {
        protected ProductAssembler productAssembler;
        protected UserAssembler userAssembler;
        protected PartnerAssembler partnerAssembler;
        protected GroupAssembler groupAssembler;
        protected WarehouseAssembler warehouseAssembler;
        protected SectorAssembler sectorAssembler;


        public ServiceBase()
        {
            productAssembler = new ProductAssembler();
            userAssembler = new UserAssembler();
            partnerAssembler = new PartnerAssembler();
            groupAssembler = new GroupAssembler();
            warehouseAssembler = new WarehouseAssembler();
            sectorAssembler = new SectorAssembler();
        }

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