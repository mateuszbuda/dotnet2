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
    [ServiceContract]
    public interface IPartnersService
    {
        [OperationContract]
        Response<List<PartnerSimpleDto>> GetPartners(Request request);

        [OperationContract]
        Response<PartnerDto> GetPartner(Request<int> partnerId);

        [OperationContract]
        Response<PartnerDto> GetPartnerByWarehouse(Request<int> warehouseId);

        [OperationContract]
        Response<List<GroupHistoryDto>> GetPartnerHistory(Request<int> partnerId);

        [OperationContract]
        Response<PartnerDto> AddNew(Request<PartnerDto> partner);

        [OperationContract]
        Response<PartnerDto> Update(Request<PartnerDto> partner);
    }
}
