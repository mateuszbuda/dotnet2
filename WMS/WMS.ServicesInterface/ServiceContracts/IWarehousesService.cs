using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.ServicesInterface.ServiceContracts
{
    [ServiceContract]
    public interface IWarehousesService
    {
        [OperationContract]
        Response<List<WarehouseSimpleDto>> GetWarehouses(Request request);
    }
}
