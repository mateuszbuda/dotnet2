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
        Response<List<ProductDto>> GetProducts(Request request);

        [OperationContract]
        Response<ProductDto> GetProduct(Request<int> ProductId);
    }
}
