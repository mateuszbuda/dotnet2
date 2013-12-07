using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.DatabaseAccess.Entities;
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
                ProductionDate = p.Date
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
    }
}