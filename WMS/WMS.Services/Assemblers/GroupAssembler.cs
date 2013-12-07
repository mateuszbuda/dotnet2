using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;

namespace WMS.Services.Assemblers
{
    public class GroupAssembler
    {
        public GroupHistoryDto ToHistoryDto(Shift s)
        {
            return new GroupHistoryDto()
            {
                Id = s.GroupId,
                Date = s.Date,
                RecipientId = s.Group.Sector.Warehouse.Id,
                RecipientName = s.Group.Sector.Warehouse.Name,
                SenderId = s.Sender.Id,
                SenderName = s.Sender.Name
            };
        }
    }
}