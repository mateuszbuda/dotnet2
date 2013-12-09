using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.ServicesInterface.DTOs;
using WMS.DatabaseAccess.Entities;
using System.ServiceModel;
using WMS.ServicesInterface.DataContracts;

namespace WMS.Services.Assemblers
{
    public class PartnerAssembler
    {
        public PartnerSimpleDto ToSimpleDto(Partner p)
        {
            return new PartnerSimpleDto()
            {
                City = p.City,
                Code = p.Code,
                Id = p.Id,
                Mail = p.Mail,
                Name = p.Warehouse.Name,
                Num = p.Num,
                Street = p.Street,
                Tel = p.Tel, 
                Version = p.Version
            };
        }

        public PartnerDto ToDto(Partner p)
        {
            return new PartnerDto()
            {
                City = p.City,
                Code = p.Code,
                Id = p.Id,
                Mail = p.Mail,
                Name = p.Warehouse.Name,
                Num = p.Num,
                Street = p.Street,
                Tel = p.Tel,
                Warehouse = new WarehouseAssembler().ToDto(p.Warehouse),
                Version = p.Version
            };
        }

        public Partner ToEntity(PartnerDto p, Partner ent = null)
        {
            if(ent != null && !p.Version.SequenceEqual(ent.Version))
                throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ent = ent ?? new Partner();

            ent.City = p.City;
            ent.Code = p.Code;
            ent.Mail = p.Mail;
            ent.Num = p.Num;
            ent.Street = p.Street;
            ent.Tel = p.Tel;
            ent.Warehouse = new WarehouseAssembler().ToEntity(p.Warehouse, ent.Warehouse);

            return ent;
        }
    }
}