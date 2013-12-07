using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;

namespace WMS.Services.Assemblers
{
    public class WarehouseAssembler
    {
        public WarehouseDto ToDto(Warehouse w)
        {
            return new WarehouseDto
            {
                City = w.City,
                Code = w.Code,
                Deleted = w.Deleted,
                Internal = w.Internal,
                Mail = w.Mail,
                Id = w.Id,
                Name = w.Name,
                Num = w.Num,
                Street = w.Street,
                Tel = w.Tel,
                Version = w.Version
            };
        }

        public WarehouseSimpleDto ToSimpleDto(Warehouse w)
        {
            int fsc = 0;
            using (var context = new SystemContext())
            {
                context.TransactionSync(tc =>
                    fsc = tc.Entities.Warehouses.Where(x => x.Id == w.Id).FirstOrDefault().
                    Sectors.Where(s => !s.Deleted).Where(s => s.Limit > s.Groups.Count).Count());
            }

            return new WarehouseSimpleDto
            {
                Id = w.Id,
                Name = w.Name,
                SectorsCount = w.Sectors.Count,
                FreeSectorsCount = fsc
            };
        }

        public Warehouse ToEntity(WarehouseDto w, Warehouse ent = null)
        {
            if (ent != null && !w.Version.SequenceEqual(ent.Version))
                throw new FaultException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz.");

            ent = ent ?? new Warehouse();

            ent.City = w.City;
            ent.Code = w.Code;
            ent.Deleted = w.Deleted;
            ent.Internal = w.Internal;
            ent.Mail = w.Mail;
            ent.Name = w.Name;
            ent.Num = w.Num;
            ent.Street = w.Street;
            ent.Tel = w.Tel;

            return ent;
        }
    }
}