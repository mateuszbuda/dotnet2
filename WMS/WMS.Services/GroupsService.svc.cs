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
            return new Response<List<GroupDto>>(SectorId.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.Latest && x.Group.SectorId == SectorId.Content).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToDto).ToList()));
        }

        public Response<GroupLocationDto> GetGroupInfo(Request<int> GroupId)
        {
            return new Response<GroupLocationDto>(GroupId.Id, Transaction(tc =>
                tc.Entities.Groups.Where(x => x.Id == GroupId.Content).
                    Include(x => x.Sector.Warehouse).
                Select(groupAssembler.ToLocationDto).FirstOrDefault()));
        }

        public Response<List<GroupHistoryDto>> GetGroupHistory(Request<int> GroupId)
        {
            return new Response<List<GroupHistoryDto>>(GroupId.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.GroupId == GroupId.Content).
                    Include(x => x.Group.Sector.Warehouse).
                Select(groupAssembler.ToHistoryDto).ToList()));
        }

        public Response<List<GroupDto>> GetGroups(Request request)
        {
            return new Response<List<GroupDto>>(request.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.Latest).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToDto).ToList()));
        }

        public Response<List<ProductDetailsDto>> GetGroupDetails(Request<int> GroupId)
        {
            return new Response<List<ProductDetailsDto>>(GroupId.Id, Transaction(tc =>
                tc.Entities.GroupsDetails.Where(x => x.GroupId == GroupId.Content).
                    Include(x => x.Product).
                Select(productAssembler.ToDetailsDto).ToList()));
        }
    }
}
