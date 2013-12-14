using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;
using System.ServiceModel;

namespace WMS.Services.Tests
{
    /// <summary>
    /// Summary description for PermissionsTest
    /// </summary>
    [TestClass]
    public class PermissionsTest : ServicesProvider
    {
        public PermissionsTest()
        {
            Level = ServicesInterface.PermissionLevel.User;
        }

        [TestMethod, ExpectedException(typeof(FaultException<ServiceException>))]
        public void WarehouseAddPermissionTest()
        {
            var w = CreateWarehouse();
            warehousesService.AddNew(new Request<WarehouseInfoDto>(new WarehouseAssembler().ToDto(w)));
        }

        [TestMethod, ExpectedException(typeof(FaultException<ServiceException>))]
        public void SectorAddPermissionTest()
        {
            var w = CreateSector();
            warehousesService.AddSector(new Request<SectorDto>(new SectorAssembler().ToDto(w)));
        }

        [TestMethod, ExpectedException(typeof(FaultException<ServiceException>))]
        public void ProductAddPermissionTest()
        {
            var w = CreateProduct();
            productsService.AddNew(new Request<ProductDto>(new ProductAssembler().ToDto(w)));
        }

        [TestMethod, ExpectedException(typeof(FaultException<ServiceException>))]
        public void PartnerAddPermissionTest()
        {
            var w = CreatePartner();
            partnersService.AddNew(new Request<PartnerDto>(new PartnerAssembler().ToDto(w)));
        }
    }
}
