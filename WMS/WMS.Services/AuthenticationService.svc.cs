﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        public Response<UserDto> Authenticate(Request<UserDto> user)
        {
            var u = userAssembler.ToEntity(user.Content);
            User ret = null;
            Transaction(tc => ret = tc.Entities.Users.Where(x => x.Username == u.Username).FirstOrDefault());

            if (ret == null || ret.Password != u.Password)
                throw new FaultException("Zły login lub hasło!");

            return new Response<UserDto>(user.Id, userAssembler.ToDto(ret));
        }
    }
}