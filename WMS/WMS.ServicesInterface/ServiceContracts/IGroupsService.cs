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
    public interface IGroupsService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<GroupDto>> GetSectorGroups(Request<int> SectorId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<GroupLocationDto> GetGroupInfo(Request<int> GroupId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<GroupDto>> GetGroupHistory(Request<int> GroupId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<GroupDto>> GetGroups(Request request);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<List<ProductDetailsDto>> GetGroupDetails(Request<int> GroupId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> IsSenderInternal(Request<GroupDto> group);
    }
}
