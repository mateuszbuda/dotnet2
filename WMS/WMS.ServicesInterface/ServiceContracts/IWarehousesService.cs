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
        [FaultContract(typeof(ServiceException))]
        Response<List<WarehouseDetailsDto>> GetWarehouses(Request request);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> DeleteIfEmpty(Request<int> warehouseId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> DeleteSectorIfEmpty(Request<int> sectorId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<StatisticsDto> GetStatistics(Request request);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<WarehouseInfoDto> GetWarehouse(Request<int> warehouseId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<SectorDto>> GetSectors(Request<int> warehouseId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<SectorDto> GetSector(Request<int> sectorId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<SectorDto> AddSector(Request<SectorDto> sector);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<WarehouseInfoDto> AddNew(Request<WarehouseInfoDto> warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<WarehouseInfoDto> Edit(Request<WarehouseInfoDto> warehouse);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<int> GetNextSectorNumber(Request<int> warehouseId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<SectorDto> EditSector(Request<SectorDto> sector);
    }
}
