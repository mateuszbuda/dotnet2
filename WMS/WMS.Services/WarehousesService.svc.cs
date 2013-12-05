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
    }
}
