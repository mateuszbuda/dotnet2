using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WMS.ServicesInterface;
using WMS.ServicesInterface.ServiceContracts;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.Services.Assemblers;
using WMS.DatabaseAccess.Entities;

namespace WMS.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class GroupsService : ServiceBase, IGroupsService
    {
        public Response<List<GroupDto>> GetSectorGroups(Request<int> SectorId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<GroupDto>>(SectorId.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.Latest && x.Group.SectorId == SectorId.Content).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToDto).ToList()));
        }

        public Response<GroupLocationDto> GetGroupInfo(Request<int> GroupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<GroupLocationDto>(GroupId.Id, Transaction(tc =>
                tc.Entities.Groups.Where(x => x.Id == GroupId.Content).
                    Include(x => x.Sector.Warehouse).
                Select(groupAssembler.ToLocationDto).FirstOrDefault()));
        }

        public Response<List<GroupDto>> GetGroupHistory(Request<int> GroupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<GroupDto>>(GroupId.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.GroupId == GroupId.Content).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToDto).ToList()));
        }

        public Response<List<GroupDto>> GetGroups(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<GroupDto>>(request.Id, Transaction(tc =>
                tc.Entities.Shifts.Where(x => x.Latest).
                    Include(x => x.Group.Sector.Warehouse).Include(x => x.Sender).
                Select(groupAssembler.ToDto).ToList()));
        }

        public Response<List<ProductDetailsDto>> GetGroupDetails(Request<int> GroupId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ProductDetailsDto>>(GroupId.Id, Transaction(tc =>
                tc.Entities.GroupsDetails.Where(x => x.GroupId == GroupId.Content).
                    Include(x => x.Product).
                Select(productAssembler.ToDetailsDto).ToList()));
        }

        public Response<GroupDto> AddNew(Request<GroupDto> group)
        {
            CheckPermissions(PermissionLevel.User);
            Shift s = null;
            Transaction(tc => s = tc.Entities.Shifts.Add(groupAssembler.ToEntity(group.Content)));
            return new Response<GroupDto>(group.Id, groupAssembler.ToDto(s));
        }

        public Response<bool> IsSenderInternal(Request<GroupDto> group)
        {
            CheckPermissions(PermissionLevel.User);
            bool ret = false;
            Transaction(tc =>
                {
                    var s = tc.Entities.Groups.Include(x => x.Shifts).Where(x => x.Id == group.Content.Id).FirstOrDefault().Shifts.Where(x => x.Latest == true).FirstOrDefault();
                    ret = tc.Entities.Warehouses.Find(s.SenderId).Internal;
                });
            return new Response<bool>(group.Id, ret);
        }
    }
}
