using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WMS.ServicesInterface.DataContracts
{
    [DataContract]
    public class Response<T>
    {
        [DataMember]
        public Guid RequestId { get; protected set; }

        [DataMember]
        public T Data { get; set; }

        public Response(Guid requestId, T data)
        {
            RequestId = requestId;
            Data = data;
        }
    }
}
