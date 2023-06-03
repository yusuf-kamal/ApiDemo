using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductsWithTypeAndBrandSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypeAndBrandSpecification(ProductSpecParams productSpecParams) : 
            base(product=>
            (string.IsNullOrEmpty(productSpecParams.Search)||product.Name.ToLower().Contains(productSpecParams.Search))&&
            (!productSpecParams.BrandId.HasValue||product.ProductBrandId==productSpecParams.BrandId)&& 
            (!productSpecParams.TypeId.HasValue || product.ProductTypeId == productSpecParams.TypeId)
                )
        {
            AddInclude(p=>p.ProductType);
            AddInclude(p=>p.ProductBrand);
            AddOrderBy(p => p.Name);
            ApplyPaging(productSpecParams.pageSize * (productSpecParams.PageIndex - 1),productSpecParams.pageSize);
            if(!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch (productSpecParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p=>p.price); break;
                    case "priceDesc":
                        AddOrderByDesc(p=>p.price); break;
                    default:
                        AddOrderBy(p=>p.Name); break;

                }
            }
        }

        public ProductsWithTypeAndBrandSpecification(int id) 
            : base(p=>p.Id==id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
