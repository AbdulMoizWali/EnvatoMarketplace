using EnvatoMarketplace.Models;
using EnvatoMarketplace.ViewModel;
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

        public ActionResult Marketplace(int? id)
        {
            var marketplaceVM = new MarketplaceVM();
            if(id != null)
            {
                try
                {
                    var category = db.Categories.First(cat => cat.catid == id);
                    if (category.pcatid == null) {
                        /*marketplaceVM.Products = db.Products.Where(product => product.catid == id);*/
                        marketplaceVM.Products = db.Products.Where(product => product.catid == id || product.Category.ParentCategory.catid == id);
                    }
                    else
                    {
                        marketplaceVM.Products = db.Products.Where(product => product.catid == id);

                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("NotFound", "Default");
                }
            }
            else
            {
                /*ModelState.AddModelError("", "All");*/
                marketplaceVM.Products = db.Products;
            }
            marketplaceVM.Categories = db.Categories;
            return View(marketplaceVM);
        }
    }
}