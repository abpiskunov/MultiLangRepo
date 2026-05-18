using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using eShopMvc.Infrastructure;
using eShopMvc.Models;

namespace eShopMvc.Services
{
    public class CatalogService
    {
        public List<CatalogItem> GetCatalogItems(int? brandFilter, int? typeFilter, int page, int pageSize)
        {
            using (var context = new CatalogContext())
            {
                IQueryable<CatalogItem> query = context.CatalogItems
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType);

                if (brandFilter.HasValue)
                {
                    query = query.Where(c => c.CatalogBrandId == brandFilter.Value);
                }

                if (typeFilter.HasValue)
                {
                    query = query.Where(c => c.CatalogTypeId == typeFilter.Value);
                }

                return query
                    .OrderBy(c => c.Name)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public CatalogItem GetCatalogItem(int id)
        {
            using (var context = new CatalogContext())
            {
                return context.CatalogItems
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .FirstOrDefault(c => c.Id == id);
            }
        }

        public List<CatalogBrand> GetBrands()
        {
            using (var context = new CatalogContext())
            {
                return context.CatalogBrands.OrderBy(b => b.Brand).ToList();
            }
        }

        public List<CatalogType> GetTypes()
        {
            using (var context = new CatalogContext())
            {
                return context.CatalogTypes.OrderBy(t => t.Type).ToList();
            }
        }
    }
}
