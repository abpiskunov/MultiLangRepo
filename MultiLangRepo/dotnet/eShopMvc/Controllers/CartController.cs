using System;
using System.Linq;
using System.Web.Mvc;
using eShopMvc.Models;
using eShopMvc.Services;

namespace eShopMvc.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController()
        {
            _cartService = new CartService();
        }

        public ActionResult Index()
        {
            string buyerId = GetOrCreateBuyerId();
            var cart = _cartService.GetCart(buyerId);
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(int productId, string productName, decimal unitPrice, string pictureUrl)
        {
            string buyerId = GetOrCreateBuyerId();
            _cartService.AddItemToCart(buyerId, productId, productName, unitPrice, pictureUrl);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveItem(string itemId)
        {
            string buyerId = GetOrCreateBuyerId();
            _cartService.RemoveItemFromCart(buyerId, itemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout()
        {
            string buyerId = GetOrCreateBuyerId();
            var cart = _cartService.GetCart(buyerId);

            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            // Simulate order placement
            _cartService.ClearCart(buyerId);
            TempData["Message"] = "Order placed successfully! Total: $" + cart.TotalPrice().ToString("F2");
            return RedirectToAction("Index", "Catalog");
        }

        private string GetOrCreateBuyerId()
        {
            if (Session["BuyerId"] == null)
            {
                Session["BuyerId"] = Guid.NewGuid().ToString();
            }
            return Session["BuyerId"].ToString();
        }
    }
}
