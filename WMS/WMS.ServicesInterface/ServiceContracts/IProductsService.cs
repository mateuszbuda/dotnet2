using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.ServicesInterface.ServiceContracts
{
    [ServiceContract]
    public interface IProductsService
    {
        [OperationContract]
        Request<List<ProductDto>> GetProducts(Request request);

        [OperationContract]
        Request<ProductDto> GetProduct(Request<int> ProductId);
    }
}
