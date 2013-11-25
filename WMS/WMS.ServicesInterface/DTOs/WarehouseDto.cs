using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    public class WarehouseDto
    {
        public int Id { get; set; }
        public bool Internal { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Num { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public bool Deleted { get; set; }
    }

    public class WarehouseSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectorsCount { get; set; }
        public int FreeSectorsCount { get; set; }
    }
}
