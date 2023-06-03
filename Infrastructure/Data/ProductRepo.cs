using Core.Entities;
using Core.Entities.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly StoreDbContext _storeDbContext;

        public ProductRepo( StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }

        public async Task<IReadOnlyList<Product>> GetProductAsync()
       =>await _storeDbContext.Products.Include(p=>p.ProductType)
            .Include(p=>p.ProductBrand).ToListAsync();

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandAsync()
        => await _storeDbContext.ProductBrands.ToListAsync();

        public async Task<Product> GetProductByIdAsync(int? id)
       => await _storeDbContext.Products.Include(p => p.ProductType)
            .Include(p => p.ProductBrand).FirstOrDefaultAsync(p => p.Id== id);

       

        public async Task<IReadOnlyList<ProductType>> GetProductTypeAsync()
        =>await _storeDbContext.ProductTypes.ToListAsync();
    }
}
