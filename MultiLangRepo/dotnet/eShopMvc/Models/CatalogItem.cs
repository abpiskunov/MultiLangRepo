using System;

namespace eShopMvc.Models
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUri { get; set; }
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }
        public int CatalogBrandId { get; set; }
        public CatalogBrand CatalogBrand { get; set; }
        public int AvailableStock { get; set; }
        public int RestockThreshold { get; set; }
        public int MaxStockThreshold { get; set; }
        public bool OnReorder { get; set; }

        public decimal DiscountedPrice(decimal discountPercent)
        {
            if (discountPercent < 0 || discountPercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(discountPercent));
            }

            return Price * (1 - discountPercent / 100m);
        }

        public void RemoveStock(int quantityDesired)
        {
            if (AvailableStock == 0)
            {
                throw new InvalidOperationException("No stock available for item " + Name);
            }

            if (quantityDesired <= 0)
            {
                throw new ArgumentException("Quantity must be positive");
            }

            int removed = Math.Min(quantityDesired, AvailableStock);
            AvailableStock -= removed;

            if (AvailableStock <= RestockThreshold)
            {
                OnReorder = true;
            }
        }

        public void AddStock(int quantity)
        {
            int original = AvailableStock;
            if (AvailableStock + quantity > MaxStockThreshold)
            {
                AvailableStock += (MaxStockThreshold - AvailableStock);
            }
            else
            {
                AvailableStock += quantity;
            }

            OnReorder = false;
        }
    }
}
