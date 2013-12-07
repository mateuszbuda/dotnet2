using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;

namespace WMS.Services.Assemblers
{
    public class SectorAssembler
    {
        public SectorDto ToDto(Sector s)
        {
            return new SectorDto
            {
                Deleted = s.Deleted,
                GroupsCount = s.Groups.Count(),
                Id = s.Id,
                Limit = s.Limit,
                Number = s.Number,
                WarehouseId = s.WarehouseId
            };
        }
    }
}