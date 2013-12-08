using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WMS.ServicesInterface;
using WMS.ServicesInterface.ServiceContracts;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;
using WMS.DatabaseAccess.Entities;

namespace WMS.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class WarehousesService : ServiceBase, IWarehousesService
    {
        private WarehouseAssembler warehouseAssembler;
        private SectorAssembler sectorAssembler;

        public WarehousesService()
        {
            warehouseAssembler = new WarehouseAssembler();
            sectorAssembler = new SectorAssembler();
        }

        public Response<List<WarehouseSimpleDto>> GetWarehouses(Request request)
        {
            return new Response<List<WarehouseSimpleDto>>(request.Id,
                Transaction(tc => tc.Entities.Warehouses.Where(x => x.Internal && !x.Deleted).ToList().
                    Select(warehouseAssembler.ToSimpleDto).ToList()));
        }

        public Response<bool> DeleteIfEmpty(Request<int> WarehouseId)
        {
            bool ret = false;

            Transaction(tc =>
                {
                    var w = (from x in tc.Entities.Warehouses
                             where x.Id == WarehouseId.Content
                             select x).FirstOrDefault();

                    int c = (from s in w.Sectors
                             where s.Deleted == false
                             where s.Groups.Count != 0
                             select s).Count();

                    if (c == 0)
                    {
                        ret = true;

                        w.Deleted = true;
                        //tc.Entities.SaveChanges();
                    }
                });

            return new Response<bool>(WarehouseId.Id, ret);
        }


        public Response<StatisticsDto> GetStatistics(Request request)
        {
            StatisticsDto stats = new StatisticsDto();
            Transaction(tc =>
                {
                    stats.WarehousesCount = (from w in tc.Entities.Warehouses
                                             where w.Internal == true
                                             where w.Deleted == false
                                             select w).Count();
                    stats.ProductsCount = tc.Entities.Products.Count();
                    stats.PartnersCount = tc.Entities.Partners.Count();
                    stats.GroupsCount = (from g in tc.Entities.Groups
                                         where g.Sector.Warehouse.Internal == true
                                         select g).Count();
                    stats.ShiftsCount = tc.Entities.Shifts.Count();
                    int all = 0;
                    int full = 0;

                    foreach (DatabaseAccess.Entities.Warehouse w in tc.Entities.Warehouses)
                        if (w.Internal == true && w.Deleted == false)
                            foreach (DatabaseAccess.Entities.Sector s in w.Sectors)
                            {
                                all += s.Limit;
                                full += s.Groups.Count;
                            }

                    stats.FIllRate = all == 0 ? 0 : (full * 100) / all;
                });

            return new Response<StatisticsDto>(request.Id, stats);
        }

        public Response<WarehouseDto> GetWarehouse(Request<int> WarehouseId)
        {
            return new Response<WarehouseDto>(WarehouseId.Id, Transaction(tc =>
                tc.Entities.Warehouses.Where(x => x.Id == WarehouseId.Content).Include(x => x.Sectors).
                Select(warehouseAssembler.ToDto).FirstOrDefault()));
        }

        public Response<List<SectorDto>> GetSectors(Request<int> WarehouseId)
        {
            return new Response<List<SectorDto>>(WarehouseId.Id, Transaction(tc =>
                tc.Entities.Sectors.Where(s =>
                    (s.WarehouseId == WarehouseId.Content && s.Deleted == false && s.Groups.Count != 0)).
                Include(x => x.Groups).
                Select(sectorAssembler.ToDto).ToList()));
        }

        public Response<SectorDto> GetSector(Request<int> SectorId)
        {
            return new Response<SectorDto>(SectorId.Id, Transaction(tc =>
                tc.Entities.Sectors.Where(s => s.Id == SectorId.Content).
                Include(x => x.Groups).
                Select(sectorAssembler.ToDto).FirstOrDefault()));
        }

        public Response<SectorDto> AddSector(Request<SectorDto> sector)
        {
            Sector s = null;
            Transaction(tc => s = tc.Entities.Sectors.Add(sectorAssembler.ToEntity(sector.Content)));
            return new Response<SectorDto>(sector.Id, sectorAssembler.ToDto(s));
        }
    }
}
