﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.DatabaseAccess.Entities;

namespace WMS.DatabaseAccess.Tests
{
    [TestClass]
    public class DatabaseAccessProvider
    {
        protected void Transaction(Action<TransactionContext> action)
        {
            using (var context = new SystemContext())
            {
                context.TransactionSync(tc => 
                { 
                    action(tc); 
                    tc.Rollback = true; 
                });
            }
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
