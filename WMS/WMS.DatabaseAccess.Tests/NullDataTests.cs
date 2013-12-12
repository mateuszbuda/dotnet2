using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using WMS.DatabaseAccess.Entities;

namespace WMS.DatabaseAccess.Tests
{
    [TestClass]
    public class NullDataTests : DatabaseAccessProvider
    {
        [TestMethod, ExpectedException(typeof(DbEntityValidationException))]
        public void Warehouse_EmptyTest()
        {
            Warehouse w = CreateWarehouse();
            w.Street = null;

            Transaction(tc =>
            {
                tc.Entities.Warehouses.Add(w);
                tc.Entities.SaveChanges();
            });
        }

        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void Sector_EmptyTest()
        {
            Sector s = CreateSector();
            s.Warehouse = null;

            Transaction(tc =>
            {
                tc.Entities.Sectors.Add(s);
                tc.Entities.SaveChanges();
            });
        }

        [TestMethod, ExpectedException(typeof(DbEntityValidationException))]
        public void Product_EmptyTest()
        {
            Product p = CreateProduct();
            p.Name = null;

            Transaction(tc =>
            {
                tc.Entities.Products.Add(p);
                tc.Entities.SaveChanges();
            });
        }

        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void Group_EmptyTest()
        {
            Group g = CreateGroup();
            g.Sector = null;

            Transaction(tc =>
            {
                tc.Entities.Groups.Add(g);
                tc.Entities.SaveChanges();
            });
        }

        [TestMethod, ExpectedException(typeof(DbEntityValidationException))]
        public void Partner_EmptyTest()
        {
            Partner p = CreatePartner();
            p.City = null;

            Transaction(tc =>
            {
                tc.Entities.Partners.Add(p);
                tc.Entities.SaveChanges();
            });
        }

        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void Shift_EmptyTest()
        {
            Shift s = CreateShift();
            s.Group = null;

            Transaction(tc =>
            {
                tc.Entities.Shifts.Add(s);
                tc.Entities.SaveChanges();
            });
        }

        [TestMethod, ExpectedException(typeof(DbUpdateException))]
        public void GroupDetails_EmptyTest()
        {
            GroupDetails g = CreateGroupDetails();
            g.Product = null;

            Transaction(tc =>
            {
                tc.Entities.GroupsDetails.Add(g);
                tc.Entities.SaveChanges();
            });
        }
    }
}
