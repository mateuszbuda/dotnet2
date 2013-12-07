using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WMS.Services.Assemblers;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProductsService.svc or ProductsService.svc.cs at the Solution Explorer and start debugging.
    public class ProductsService : ServiceBase, IProductsService
    {
        private ProductAssembler productAssembler;

        public ProductsService()
        {
            productAssembler = new ProductAssembler();
        }

        public Response<List<ProductDto>> GetProducts(Request request)
        {
            return new Response<List<ProductDto>>(request.Id, Transaction(tc => 
                tc.Entities.Products.Select(productAssembler.ToDto).ToList()));
        }

        public Response<ProductDto> GetProduct(Request<int> ProductId)
        {
            return new Response<ProductDto>(ProductId.Id, Transaction(tc =>
                productAssembler.ToDto(tc.Entities.Products.Find(ProductId.Content))));
        }
    }
}
