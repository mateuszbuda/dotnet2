using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace WMS.Services.Assemblers
{
    public class UserAssembler
    {
        public UserDto ToDto(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Permissions = user.Permissions,
                Password = ""
            };
        }

        public User ToEntity(UserDto user)
        {
            byte[] value;
            byte [] input = Encoding.UTF8.GetBytes(user.Password);
            SHA256 hash = SHA256Managed.Create();
            value = hash.ComputeHash(input);
            StringBuilder pwd = new StringBuilder();

            foreach(byte x in value)
                pwd.Append(x.ToString("x2"));

            return new User()
            {
                Username = user.Username,
                Password = pwd.ToString()
            };
        }
    }
}