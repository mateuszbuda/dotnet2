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
        [FaultContract(typeof(ServiceException))]
        Response<List<ProductDto>> GetProducts(Request request);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<ProductDto> GetProduct(Request<int> ProductId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> AddNew(Request<ProductDto> product);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Response<bool> Edit(Request<ProductDto> product);
    }
}
