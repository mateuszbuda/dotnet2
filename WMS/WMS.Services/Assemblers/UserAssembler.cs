using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;
using System.Security.Cryptography;
using System.Text;
using WMS.ServicesInterface;

namespace WMS.Services.Assemblers
{
    /// <summary>
    /// Klasa odpowiada za konwersję pomiędzy obiektami związanymi z użytkownikami
    /// mapowanymi do bazy a obiektami-paczkami przesyłanymi do i z serwera.
    /// </summary>
    public class UserAssembler
    {
        /// <summary>
        /// Konwersja z bazodanowego użytkownika na użytkownika-paczkę do komunikacji z serwerem.
        /// </summary>
        /// <param name="user">Konwertowany użytkownik</param>
        /// <returns>Przekonwertowany uzytkownik</returns>
        public UserDto ToDto(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Permissions = (PermissionLevel)user.Permissions,
                Password = ""
            };
        }

        /// <summary>
        /// Konwersja z użytkownika-paczki do komunikacji z serwerem na użytkownika bazodanowy.
        /// </summary>
        /// <param name="user">Konwertowany użytkownik</param>
        /// <returns>Przekonwertowany sektor użytkownik</returns>
        public User ToEntity(UserDto user)
        {
            // TODO - dlaczego tutaj nie psrawdzamy entity
            byte[] value;
            byte[] input = Encoding.UTF8.GetBytes(user.Password);
            SHA256 hash = SHA256Managed.Create();
            value = hash.ComputeHash(input);
            StringBuilder pwd = new StringBuilder();

            foreach (byte x in value)
                pwd.Append(x.ToString("x2"));

            return new User()
            {
                Username = user.Username,
                Password = pwd.ToString()
            };
        }
    }
}