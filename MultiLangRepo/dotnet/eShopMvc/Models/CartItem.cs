using System;
using System.Collections.Generic;

namespace eShopMvc.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }

        public decimal TotalPrice
        {
            get { return UnitPrice * Quantity; }
        }
    }

    public class ShoppingCart
    {
        public string BuyerId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalPrice()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.TotalPrice;
            }
            return total;
        }

        public void AddItem(int productId, string productName, decimal unitPrice, string pictureUrl, int quantity = 1)
        {
            var existingItem = Items.Find(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = productId,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    PictureUrl = pictureUrl,
                    Quantity = quantity
                });
            }
        }

        public void RemoveItem(string itemId)
        {
            Items.RemoveAll(i => i.Id == itemId);
        }
    }

    public class ApplicationUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
