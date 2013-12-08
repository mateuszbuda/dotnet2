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

        [OperationContract]
        Response<bool> DeleteIfEmpty(Request<int> warehouseId);

        [OperationContract]
        Response<bool> DeleteSectorIfEmpty(Request<int> sectorId);

        [OperationContract]
        Response<StatisticsDto> GetStatistics(Request request);

        [OperationContract]
        Response<WarehouseDto> GetWarehouse(Request<int> warehouseId);

        [OperationContract]
        Response<List<SectorDto>> GetSectors(Request<int> warehouseId);

        [OperationContract]
        Response<SectorDto> GetSector(Request<int> sectorId);

        [OperationContract]
        Response<SectorDto> AddSector(Request<SectorDto> sector);
    }
}
