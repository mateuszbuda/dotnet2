using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess;
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

        public GroupDto ToDto(Group g)
        {
            DateTime date = new DateTime();
            return new GroupDto()
            {
                Date = date,
                Id = g.Id,
                Internal = g.Sector.Warehouse.Internal,
                SenderId = g.Sector.WarehouseId,
                SenderName = g.Sector.Warehouse.Name,
                WarehouseId = new Random().Next(),
                WarehouseName = "no nie mogę...",
            };
        }
    }
}