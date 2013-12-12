using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WMS.Services.Assemblers;
using WMS.ServicesInterface;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;
using WMS.ServicesInterface.ServiceContracts;

namespace WMS.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class ProductsService : ServiceBase, IProductsService
    {
        public Response<List<ProductDto>> GetProducts(Request request)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<List<ProductDto>>(request.Id, Transaction(tc => 
                tc.Entities.Products.Select(productAssembler.ToDto).ToList()));
        }

        public Response<ProductDto> GetProduct(Request<int> ProductId)
        {
            CheckPermissions(PermissionLevel.User);
            return new Response<ProductDto>(ProductId.Id, Transaction(tc =>
                productAssembler.ToDto(tc.Entities.Products.Find(ProductId.Content))));
        }

        public Response<bool> AddNew(Request<ProductDto> product)
        {
            CheckPermissions(PermissionLevel.Manager);
            Transaction(tc => tc.Entities.Products.Add(productAssembler.ToEntity(product.Content)));
            return new Response<bool>(product.Id, true);
        }

        public Response<bool> Edit(Request<ProductDto> product)
        {
            CheckPermissions(PermissionLevel.Manager);
            Transaction(tc =>
                {
                    var p = tc.Entities.Products.Find(product.Content.Id);
                    if (p == null)
                        throw new FaultException<ServiceException>(new ServiceException("Ten produkt nie istnieje!"));

                    productAssembler.ToEntity(product.Content, p);
                });
            return new Response<bool>(product.Id, true);
        }
    }
}
