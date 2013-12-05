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
    }
}
