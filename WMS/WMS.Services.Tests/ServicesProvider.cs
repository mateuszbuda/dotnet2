using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;

namespace WMS.Services.Tests
{
    [TestClass]
    public class ServicesProvider
    {
        protected WarehousesService warehousesService;
        protected ProductsService productsService;
        protected PartnersService partnersService;
        protected GroupsService groupsService;
        protected AuthenticationService authenticationService;

        protected PermissionLevel Level { get; set; }

        private void PermissionChecker(PermissionLevel level)
        {
            if (Level > level)
                throw new FaultException<ServiceException>(new ServiceException("error"));
        }

        protected ServicesProvider()
        {
            warehousesService = new WarehousesService();
            productsService = new ProductsService();
            partnersService = new PartnersService();
            groupsService = new GroupsService();
            authenticationService = new AuthenticationService();

            warehousesService.Rollback = true;
            productsService.Rollback = true;
            partnersService.Rollback = true;
            groupsService.Rollback = true;
            authenticationService.Rollback = true;

            warehousesService.CheckPermissions = PermissionChecker;
            productsService.CheckPermissions = PermissionChecker;
            partnersService.CheckPermissions = PermissionChecker;
            groupsService.CheckPermissions = PermissionChecker;
            authenticationService.CheckPermissions = PermissionChecker;

            Level = PermissionLevel.Administrator;
        }


        protected Warehouse CreateWarehouse()
        {
            return new Warehouse()
            {
                Name = "Warehouse",
                City = "X",
                Code = "00-000",
                Deleted = false,
                Internal = true,
                Mail = "test@test.com",
                Num = "123A",
                Street = "X",
                Tel = "+48000000000"
            };
        }

        protected Sector CreateSector(int limit = 1)
        {
            return new Sector()
            {
                Deleted = false,
                Limit = limit,
                Number = 1,
                Warehouse = CreateWarehouse()
            };
        }

        protected Product CreateProduct()
        {
            return new Product()
            {
                Name = "N",
                Date = DateTime.Now,
                Price = 12M
            };
        }

        protected Group CreateGroup()
        {
            return new Group()
            {
                Sector = CreateSector()
            };
        }

        protected GroupDetails CreateGroupDetails()
        {
            return new GroupDetails()
            {
                Count = 1,
                Product = CreateProduct(),
                Group = CreateGroup()
            };
        }

        protected Partner CreatePartner()
        {
            Partner p = new Partner()
            {
                Tel = "0",
                City = "a",
                Code = "0",
                Mail = "a",
                Num = "0",
                Street = "a",
                Warehouse = CreateWarehouse()
            };

            p.Warehouse.Sectors = new HashSet<Sector>();
            Sector s = CreateSector(0);
            s.Warehouse = null;
            p.Warehouse.Sectors.Add(s);

            return p;
        }

        protected Shift CreateShift()
        {
            return new Shift()
            {
                Date = DateTime.Now,
                Group = CreateGroup(),
                Sender = CreateWarehouse(),
                Latest = true
            };
        }
    }
}
