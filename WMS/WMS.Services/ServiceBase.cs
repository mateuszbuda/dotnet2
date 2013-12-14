using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.Services.Assemblers;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;

namespace WMS.Services
{
    public delegate void CheckPermissionsDelegate(PermissionLevel permission);

    public class ServiceBase
    {
        protected ProductAssembler productAssembler;
        protected UserAssembler userAssembler;
        protected PartnerAssembler partnerAssembler;
        protected GroupAssembler groupAssembler;
        protected WarehouseAssembler warehouseAssembler;
        protected SectorAssembler sectorAssembler;

        public bool Rollback { get; set; }

        public ServiceBase()
        {
            productAssembler = new ProductAssembler();
            userAssembler = new UserAssembler();
            partnerAssembler = new PartnerAssembler();
            groupAssembler = new GroupAssembler();
            warehouseAssembler = new WarehouseAssembler();
            sectorAssembler = new SectorAssembler();

            CheckPermissions = DefaultPermissionChecker;
            Rollback = false;
        }

        public CheckPermissionsDelegate CheckPermissions { get; set; }

        protected T Transaction<T>(Func<TransactionContext, T> action)
        {
            using (var context = new SystemContext())
            {
                return context.TransactionSync(tc =>
                    {
                        if (Rollback)
                            tc.Rollback = true;
                        return action(tc);
                    });
            }
        }

        protected void Transaction(Action<TransactionContext> action)
        {
            using (var context = new SystemContext())
            {
                context.TransactionSync(tc =>
                    {
                        if (Rollback)
                            tc.Rollback = true;
                        action(tc);
                    });
            }
        }

        protected void DefaultPermissionChecker(PermissionLevel permission)
        {
            User u = null;
            Transaction(tc => u = tc.Entities.Users.
                Where(x => x.Username == ServiceSecurityContext.Current.PrimaryIdentity.Name).FirstOrDefault());

            if (u == null || u.Permissions > (int)permission)
                throw new FaultException<ServiceException>(new ServiceException("Brak uprawnień!"));
        }
    }
}