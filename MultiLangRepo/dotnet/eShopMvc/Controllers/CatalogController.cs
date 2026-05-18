using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eShopMvc.Models;
using eShopMvc.Services;

namespace eShopMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogService;

        public CatalogController()
        {
            _catalogService = new CatalogService();
        }

        public ActionResult Index(int? brandFilter, int? typeFilter, int page = 0)
        {
            int pageSize = 12;
            var items = _catalogService.GetCatalogItems(brandFilter, typeFilter, page, pageSize);
            var brands = _catalogService.GetBrands();
            var types = _catalogService.GetTypes();

            ViewBag.Brands = new SelectList(brands, "Id", "Brand", brandFilter);
            ViewBag.Types = new SelectList(types, "Id", "Type", typeFilter);
            ViewBag.CurrentPage = page;
            ViewBag.BrandFilter = brandFilter;
            ViewBag.TypeFilter = typeFilter;

            return View(items);
        }

        public ActionResult Details(int id)
        {
            var item = _catalogService.GetCatalogItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            var item = _catalogService.GetCatalogItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            // In a real app this would use session/cookie-based cart
            TempData["Message"] = string.Format("Added {0} x {1} to cart", quantity, item.Name);
            return RedirectToAction("Index");
        }
    }
}
