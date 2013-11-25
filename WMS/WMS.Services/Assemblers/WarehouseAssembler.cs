using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                Tel = w.Tel
            };
        }

        public WarehouseSimpleDto ToSimpleDto(Warehouse w)
        {
            return new WarehouseSimpleDto
            {
                Id = w.Id,
                Name = w.Name,
                SectorsCount = w.Sectors.Count,
                FreeSectorsCount = 0
            };
        }
    }
}