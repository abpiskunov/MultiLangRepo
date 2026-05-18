using System.Collections.Concurrent;
using eShopMvc.Models;

namespace eShopMvc.Services
{
    public class CartService
    {
        private static readonly ConcurrentDictionary<string, ShoppingCart> _carts =
            new ConcurrentDictionary<string, ShoppingCart>();

        public ShoppingCart GetCart(string buyerId)
        {
            return _carts.GetOrAdd(buyerId, id => new ShoppingCart { BuyerId = id });
        }

        public void AddItemToCart(string buyerId, int productId, string productName, decimal unitPrice, string pictureUrl)
        {
            var cart = GetCart(buyerId);
            cart.AddItem(productId, productName, unitPrice, pictureUrl);
        }

        public void RemoveItemFromCart(string buyerId, string itemId)
        {
            var cart = GetCart(buyerId);
            cart.RemoveItem(itemId);
        }

        public void ClearCart(string buyerId)
        {
            ShoppingCart removed;
            _carts.TryRemove(buyerId, out removed);
        }
    }
}
