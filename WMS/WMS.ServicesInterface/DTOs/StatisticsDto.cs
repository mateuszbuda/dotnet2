using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    public class StatisticsDto
    {
        public int WarehousesCount { get; set; }
        public int ProductsCount { get; set; }
        public int PartnersCount { get; set; }
        public int GroupsCount { get; set; }
        public int ShiftsCount { get; set; }
        public int FIllRate { get; set; }
    }
}
