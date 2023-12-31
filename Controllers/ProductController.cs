using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnvatoMarketplace.Controllers
{
    public class ProductController : Controller
    {
        EnvatoDBEntities db = new EnvatoDBEntities();
        // GET: Product
        public ActionResult Index(int? id)
        {
            try
            {
                Product product = db.Products.First(prod => prod.pid == id);
                return View(product);
            }
            catch(Exception ex) {
                return RedirectToAction("NotFound", "Default");
            }
        }
    }
}