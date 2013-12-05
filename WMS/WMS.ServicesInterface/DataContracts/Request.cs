using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.DataContracts
{
    [DataContract]
    public class Request
    {
        [DataMember]
        private readonly Guid _id = Guid.NewGuid();

        public Guid Id
        {
            get { return _id; }
        }
    }

    [DataContract]
    public class Request<T>
    {
        public Request(T content)
        {
            Content = content;
        }

        [DataMember]
        public T Content { get; set; }
    }
}
