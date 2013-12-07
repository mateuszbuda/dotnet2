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

namespace WMS.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class WarehousesService : ServiceBase, IWarehousesService
    {
        WarehouseAssembler warehouseAssembler;

        public WarehousesService()
        {
            warehouseAssembler = new WarehouseAssembler();
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
                        tc.Entities.SaveChanges();
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
            WarehouseDto warehouse = new WarehouseDto();
            Transaction(tc =>
                {
                    DatabaseAccess.Entities.Warehouse w = (from x in tc.Entities.Warehouses
                                                           where x.Id == WarehouseId.Content
                                                           select x).FirstOrDefault();
                    // Może można to było jakoś mądrzej zrobić, ale wolałem nie ryzykować...
                    warehouse.City = w.City;
                    warehouse.Code = w.City;
                    warehouse.Deleted = w.Deleted;
                    warehouse.Id = w.Id;
                    warehouse.Internal = w.Internal;
                    warehouse.Mail = w.Mail;
                    warehouse.Name = w.Name;
                    warehouse.Num = w.Num;
                    warehouse.Street = w.Street;
                    warehouse.Tel = w.Tel;
                });

            return new Response<WarehouseDto>(WarehouseId.Id, warehouse);
        }

        public Response<List<SectorDto>> GetSectors(Request<int> WarehouseId)
        {
            List<SectorDto> sectors = new List<SectorDto>();

            Transaction(tc =>
                {
                    DatabaseAccess.Entities.Warehouse w = (from x in tc.Entities.Warehouses
                                                           where x.Id == WarehouseId.Content
                                                           select x).FirstOrDefault();

                    List<DatabaseAccess.Entities.Sector> sec = (from s in w.Sectors
                                                                where s.Deleted == false
                                                                where s.Groups.Count != 0
                                                                select s).ToList();

                    foreach (DatabaseAccess.Entities.Sector s in sec)
                        sectors.Add(new SectorDto(s.Id, s.Number, s.Limit, s.Deleted, s.WarehouseId, s.Groups.Count));
                });

            return new Response<List<SectorDto>>(WarehouseId.Id, sectors);
        }

        public Response<SectorDto> GetSector(Request<int> SectorId)
        {
            throw new NotImplementedException();
        }
    }
}
