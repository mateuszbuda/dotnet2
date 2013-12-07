using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.ServicesInterface;
using WMS.ServicesInterface.ServiceContracts;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.ServicesInterface.DTOs
{
    public class SectorDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Limit { get; set; }
        public bool Deleted { get; set; }
        public int WarehouseId { get; set; }
        public int GroupsCount { get; set; } // info ile zajete
    }
}
