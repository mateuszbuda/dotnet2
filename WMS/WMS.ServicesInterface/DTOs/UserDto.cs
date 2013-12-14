using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DTOs
{
    /// <summary>
    /// Paczka z informacjami o urzytkownikach
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Nazwa użytkownika
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Hasło uzytkownika (hash)
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Id użytkownika
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Typ konta (okraśla prawa uzytkownika)
        /// </summary>
        public PermissionLevel Permissions { get; set; }
    }
}
