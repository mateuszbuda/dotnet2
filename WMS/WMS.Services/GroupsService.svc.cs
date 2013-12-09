using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.Entity;
using WMS.Services.Assemblers;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class GroupsService : ServiceBase, IGroupsService
    {
        public Response<List<GroupDto>> GetSectorGroups(Request<int> SectorId)
        {
            throw new NotImplementedException();
        }

        public Response<GroupLocationDto> GetGroupInfo(Request<int> GroupId)
        {
            throw new NotImplementedException();
        }

        public Response<List<GroupHistoryDto>> GetGroupHistory(Request<int> GroupId)
        {
            throw new NotImplementedException();
        }

        public Response<List<GroupDto>> GetGroups(Request request)
        {
            throw new NotImplementedException();
        }

        public Response<List<ProductDetailsDto>> GetGroupDetails(Request<int> GroupId)
        {
            throw new NotImplementedException();
        }
    }
}
