using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WMS.DatabaseAccess.Entities;
using WMS.ServicesInterface.DataContracts;
using WMS.ServicesInterface.DTOs;

namespace WMS.Services.Assemblers
{
    public class ProductAssembler
    {
        public ProductDto ToDto(Product p)
        {
            return new ProductDto()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ProductionDate = p.Date,
                Version = p.Version
            };
        }

        public ProductDetailsDto ToDetailsDto(GroupDetails g)
        {
            return new ProductDetailsDto()
            {
                Id = g.Product.Id,
                Name = g.Product.Name,
                Price = g.Product.Price,
                ProductionDate = g.Product.Date,
                Count = g.Count
            };
        }

        public Product ToEntity(ProductDto p, Product ent = null)
        {
            if (ent != null && !p.Version.SequenceEqual(ent.Version))
                throw new FaultException<ServiceException>(new ServiceException("Ktoś przed chwilą zmodyfikował dane.\nSpróbuj jeszcze raz."));

            ent = ent ?? new Product();

            ent.Name = p.Name;
            ent.Date = p.ProductionDate;
            ent.Price = p.Price;

            return ent;
        }
    }
}