using System.Collections.Generic;
using System.Data.Entity;
using eShopMvc.Models;

namespace eShopMvc.Infrastructure
{
    public class CatalogSeed : CreateDatabaseIfNotExists<CatalogContext>
    {
        protected override void Seed(CatalogContext context)
        {
            var brands = new List<CatalogBrand>
            {
                new CatalogBrand { Id = 1, Brand = "Azure" },
                new CatalogBrand { Id = 2, Brand = ".NET" },
                new CatalogBrand { Id = 3, Brand = "Visual Studio" },
                new CatalogBrand { Id = 4, Brand = "SQL Server" },
                new CatalogBrand { Id = 5, Brand = "Other" }
            };
            brands.ForEach(b => context.CatalogBrands.Add(b));

            var types = new List<CatalogType>
            {
                new CatalogType { Id = 1, Type = "Mug" },
                new CatalogType { Id = 2, Type = "T-Shirt" },
                new CatalogType { Id = 3, Type = "Sheet" },
                new CatalogType { Id = 4, Type = "USB Memory Stick" }
            };
            types.ForEach(t => context.CatalogTypes.Add(t));

            var items = new List<CatalogItem>
            {
                new CatalogItem { Name = ".NET Bot Black Hoodie", Description = "A warm hoodie with the .NET Bot on it", Price = 19.5m, PictureUri = "1.png", CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, MaxStockThreshold = 200, RestockThreshold = 10 },
                new CatalogItem { Name = ".NET Black & White Mug", Description = "Classic .NET mug", Price = 8.50m, PictureUri = "2.png", CatalogTypeId = 1, CatalogBrandId = 2, AvailableStock = 89, MaxStockThreshold = 200, RestockThreshold = 10 },
                new CatalogItem { Name = "Azure T-Shirt", Description = "Show your Azure love", Price = 12.00m, PictureUri = "3.png", CatalogTypeId = 2, CatalogBrandId = 1, AvailableStock = 56, MaxStockThreshold = 150, RestockThreshold = 10 },
                new CatalogItem { Name = "VS Code USB Stick 16GB", Description = "VS Code branded USB memory stick", Price = 15.00m, PictureUri = "4.png", CatalogTypeId = 4, CatalogBrandId = 3, AvailableStock = 120, MaxStockThreshold = 300, RestockThreshold = 20 },
                new CatalogItem { Name = "SQL Server Sheet", Description = "SQL Server themed sheet", Price = 8.00m, PictureUri = "5.png", CatalogTypeId = 3, CatalogBrandId = 4, AvailableStock = 34, MaxStockThreshold = 100, RestockThreshold = 5 },
            };
            items.ForEach(i => context.CatalogItems.Add(i));

            context.SaveChanges();
        }
    }
}
