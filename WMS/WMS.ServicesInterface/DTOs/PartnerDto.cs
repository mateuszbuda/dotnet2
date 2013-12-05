using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    public class PartnerDto : PartnerSimpleDto // dane magazynu partnera
    {
        public WarehouseDto Warehouse { get; set; }
    }

    public class PartnerSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Code { get; set; }
        public string Street { get; set; }
        public string Num { get; set; }
        public string Tel { get; set; }
        public string Mail { get; set; }
    }
}
