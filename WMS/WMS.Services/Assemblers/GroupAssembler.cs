using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;
using System.Data.Entity;

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
                SenderName = s.Sender.Name,
                Internal = s.Group.Sector.Warehouse.Internal,
            };
        }

        public GroupDto ToDto(Shift s)
        {
            int warehouseId = 0;
            string warehouseName = "";
            int groupId = s.GroupId;

            if (s.Latest)
            {
                warehouseId = s.Group.Sector.WarehouseId;
                warehouseName = s.Group.Sector.Warehouse.Name;
            }
            else
            {
                List<Shift> shifts;
                using (var context = new SystemContext())
                {
                    context.TransactionSync(tc =>
                        {
                            shifts = tc.Entities.Shifts.Include(x => x.Sender).Where(x => x.GroupId == groupId).ToList().
                                FindAll(x => x.Date > s.Date);
                            shifts.Sort((x, y) =>
                                {
                                    if (x.Date < y.Date) return -1;
                                    else if (x.Date > y.Date) return 1;
                                    return 0;
                                });
                            warehouseId = shifts[0].SenderId;
                            warehouseName = shifts[0].Sender.Name;
                        });
                }
            }

            return new GroupDto()
            {
                Date = s.Date,
                Id = s.GroupId,
                Internal = s.Group.Sector.Warehouse.Internal,
                SenderId = s.SenderId,
                SenderName = s.Sender.Name,
                WarehouseId = warehouseId,
                WarehouseName = warehouseName,
            };
        }

        public GroupLocationDto ToLocationDto(Group g)
        {
            return new GroupLocationDto()
            {
                Id = g.Id,
                Internal = g.Sector.Warehouse.Internal,
                SectorNumber = g.Sector.Number,
                WarehouseName = g.Sector.Warehouse.Name,
            };
        }
    }
}