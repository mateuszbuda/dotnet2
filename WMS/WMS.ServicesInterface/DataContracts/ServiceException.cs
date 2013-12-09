using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DataContracts
{
    [DataContract]
    public class ServiceException
    {
        public ServiceException(string m)
        {
            Message = m;
        }

        [DataMember]
        public string Message { get; set; }
    }
}
