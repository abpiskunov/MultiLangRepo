using System.Data.Entity;
using eShopMvc.Models;

namespace eShopMvc.Infrastructure
{
    public class CatalogContext : DbContext
    {
        public CatalogContext() : base("name=CatalogConnection")
        {
        }

        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogItem>()
                .HasRequired(c => c.CatalogBrand)
                .WithMany()
                .HasForeignKey(c => c.CatalogBrandId);

            modelBuilder.Entity<CatalogItem>()
                .HasRequired(c => c.CatalogType)
                .WithMany()
                .HasForeignKey(c => c.CatalogTypeId);

            modelBuilder.Entity<CatalogItem>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
