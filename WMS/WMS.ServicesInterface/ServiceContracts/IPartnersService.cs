using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.ServicesInterface.ServiceContracts
{
    /// <summary>
    /// Interfejs do wymiany informacji o partnerach
    /// </summary>
    [ServiceContract]
    public interface IPartnersService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<PartnerSimpleDto>> GetPartners(Request request);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<PartnerDto> GetPartner(Request<int> PartnerId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<ShiftHistoryDto>> GetPartnerHistory(Request<int> PartnerId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<PartnerDto> GetPartnerByWarehouse(Request<int> warehouseId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<PartnerDto> AddNew(Request<PartnerDto> partner);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<PartnerDto> Update(Request<PartnerDto> partner);
    }
}
