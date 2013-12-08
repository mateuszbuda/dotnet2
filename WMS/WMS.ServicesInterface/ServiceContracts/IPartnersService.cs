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
        Response<PartnerDto> GetPartner(Request<int> PartnerId);

        [OperationContract]
        Response<List<GroupHistoryDto>> GetPartnerHistory(Request<int> PartnerId);

        [OperationContract]
        Response<PartnerDto> AddNew(Request<PartnerDto> partner);

        [OperationContract]
        Response<PartnerDto> Update(Request<PartnerDto> partner);
    }
}
