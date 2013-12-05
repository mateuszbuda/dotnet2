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
    interface IGroupsService
    {
        [OperationContract]
        Response<List<GroupDto>> GetSectorGroups(Request<int> SectorId);

        [OperationContract]
        Response<GroupLocationDto> GetGroupInfo(Request<int> GroupId);

        [OperationContract]
        Response<List<GroupHistoryDto>> GetGroupHistory(Request<int> GroupId);

        [OperationContract]
        Response<List<GroupDto>> GetGroups(Request request);

        [OperationContract]
        Response<List<ProductDetailsDto>> GetGroupDetails(Request<int> GroupId);
    }
}
