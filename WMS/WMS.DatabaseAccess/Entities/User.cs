using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.DatabaseAccess.Entities
{
    public class User
    {
        [Key, Required]
        public int Id { get; set; }

        [Column("login"), Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int Permissions { get; set; }
    }
}
