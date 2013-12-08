using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;

namespace WMS.Services.Assemblers
{
    public class SectorAssembler
    {
        public Sector ToEntity(SectorDto s, Sector ent = null)
        {
            if (ent != null && !s.Version.SequenceEqual(ent.Version))
                throw new FaultException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz.");

            ent = ent ?? new Sector();

            ent.Deleted = s.Deleted;
            ent.Limit = s.Limit;
            ent.Number = s.Number;
            ent.WarehouseId = s.WarehouseId;

            return ent;
        }

        public SectorDto ToDto(Sector s)
        {
            int gc = s.Groups == null ? -1 : s.Groups.Count;

            return new SectorDto
            {
                Deleted = s.Deleted,
                GroupsCount = gc,
                Id = s.Id,
                Limit = s.Limit,
                Number = s.Number,
                WarehouseId = s.WarehouseId,
                WarehouseName = s.Warehouse.Name,
                Version = s.Version
            };
        }
    }
}