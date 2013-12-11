using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    public class GroupDto
    {
        public int Id { get; set; }
        // id magazynu nadawcy
        public int SenderId { get; set; }
        // nazwa magazynu nadawcy
        public string SenderName { get; set; }
        public DateTime Date { get; set; }
        // id magazynu odbiorcy
        public int WarehouseId { get; set; }
        // nazwa magazynu odbiorcy
        public string WarehouseName { get; set; }
        public bool Internal { get; set; }
        public byte[] Version { get; set; }
    }

    public class GroupLocationDto // do użycia w oknie 5 jako info o grupie
    {
        public int Id { get; set; }
        public string WarehouseName { get; set; }
        public int SectorNumber { get; set; }
        public bool Internal { get; set; }
    }

    public class GroupHistoryDto // okno 6, 9
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public DateTime Date { get; set; }
        public bool Internal { get; set; }
    }
}
