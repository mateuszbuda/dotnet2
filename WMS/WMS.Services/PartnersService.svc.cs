﻿using System;
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
    public class PartnersService : ServiceBase, IPartnersService
    {
        private PartnerAssembler partnerAssembler;
        private GroupAssembler groupAssembler;

        public PartnersService()
        {
            partnerAssembler = new PartnerAssembler();
            groupAssembler = new GroupAssembler();
        }

        public Response<List<PartnerSimpleDto>> GetPartners(Request request)
        {
            return new Response<List<PartnerSimpleDto>>(request.Id, Transaction(tc => tc.Entities.Partners.Include(x => x.Warehouse).
                Select(partnerAssembler.ToSimpleDto).ToList()));
        }

        public Response<PartnerDto> GetPartner(Request<int> PartnerId)
        {
            return new Response<PartnerDto>(PartnerId.Id, Transaction(tc => 
                tc.Entities.Partners.Where(x => x.Id == PartnerId.Content).Include(x => x.Warehouse).
                Select(partnerAssembler.ToDto).FirstOrDefault()));
        }

        public Response<List<GroupHistoryDto>> GetPartnerHistory(Request<int> PartnerId)
        {
            return new Response<List<GroupHistoryDto>>(PartnerId.Id, Transaction(tc =>
                {
                    int wId = tc.Entities.Partners.Where(x => x.Id == PartnerId.Content).Select(x => x.WarehouseId).FirstOrDefault();
                    return tc.Entities.Shifts.Where(s => (s.SenderId == wId || s.Group.Sector.WarehouseId == wId)).
                    Include(x => x.Sender).Include(x => x.Group.Sector.Warehouse).Select(groupAssembler.ToHistoryDto).ToList();
                }));
        }
    }
}