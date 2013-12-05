using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ProductionDate { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductDetailsDto : ProductDto  // do użycia w podglądzie Partii (okno 5)
    {
        public int Count { get; set; }
    }
}
