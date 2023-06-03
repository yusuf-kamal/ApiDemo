using Core.Entities;
using Core.Entities.OrderAgregate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreDbContextSeed
    {
        public static async Task SeedAsync(StoreDbContext storeDbContext, ILoggerFactory loggerFactory)
        {
			try
			{
				if(storeDbContext.ProductBrands!= null && !storeDbContext.ProductBrands.Any())
				{
					var branddata = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
					var brands = JsonSerializer.Deserialize<List<ProductBrand>>(branddata);
					foreach (var item in brands)
						storeDbContext.Add(item);
					await storeDbContext.SaveChangesAsync();
				}


                if (storeDbContext.ProductTypes != null && !storeDbContext.ProductTypes.Any())
                {
                    var Typesdata = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(Typesdata);
                    foreach (var item in Types)
                        storeDbContext.Add(item);
                    await storeDbContext.SaveChangesAsync();
                }


                if (storeDbContext.Products != null && !storeDbContext.Products.Any())
                {
                    var Productsdata = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(Productsdata);
                    foreach (var item in products)
                        storeDbContext.Add(item);
                    await storeDbContext.SaveChangesAsync();
                }



                if (storeDbContext.DeliveryMethods != null && !storeDbContext.DeliveryMethods.Any())
                {
                    var DeliveryMethodsDate = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsDate);
                    foreach (var item in DeliveryMethods)
                        storeDbContext.Add(item);
                    await storeDbContext.SaveChangesAsync();
                }


            }
            catch (Exception ex)
			{

				var logger=loggerFactory.CreateLogger<StoreDbContext>();
                logger.LogError(ex.Message);
			}
        }
    }
}
