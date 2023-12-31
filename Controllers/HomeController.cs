using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnvatoMarketplace.Controllers
{
    public class HomeController : Controller
    {
        EnvatoDBEntities db = new EnvatoDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Marketplace()
        {
            var marketplaceVM = new MarketplaceVM();
            marketplaceVM.Products = db.Products;
            marketplaceVM.Categories = db.Categories;
            return View(marketplaceVM);
        }
    }
}