using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.ServicesInterface.DTOs;
using WMS.DatabaseAccess.Entities;

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
                Tel = p.Tel
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
                Warehouse = new WarehouseAssembler().ToDto(p.Warehouse)
            };
        }
    }
}