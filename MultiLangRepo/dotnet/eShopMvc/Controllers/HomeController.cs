using System.Web.Mvc;

namespace eShopMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "eShop on .NET Framework";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "eShop sample application built with ASP.NET MVC 5 and Entity Framework 6.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";
            return View();
        }

        public ActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
