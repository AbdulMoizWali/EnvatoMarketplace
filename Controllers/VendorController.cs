using EnvatoMarketplace.Models;
using EnvatoMarketplace.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnvatoMarketplace.Controllers
{
    public class VendorController : Controller
    {
        EnvatoDBEntities db = new EnvatoDBEntities();
        // GET: Vendor
        public ActionResult Index()
        {
            List<Product> products = db.Products.ToList();
            List<User> users = db.Users.ToList();



            var vendorId = Convert.ToInt32(Session["uid"]);

            var vendor = users.Where(x => x.uid == vendorId).FirstOrDefault();

            if (vendor != null)
            {
                var listedProducts = products.Where(x => x.uid == vendor.uid);

                VendorProductVM vendorProductVM = new VendorProductVM
                {
                    Users = users,
                    Products = listedProducts,
                };
                return View(vendorProductVM);
            }

            return RedirectToAction("NotFound", "Default");

        }


        public ActionResult DeleteProduct(int? productId)
        {
            if (productId == null)
            {
                return RedirectToAction("Index");
            }

            string username = Session["username"]?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index");
            }

            var vendor = db.Users
                .Where(x => x.username == username)
                .FirstOrDefault();

            if (vendor == null || vendor.uid == 0)
            {
                return RedirectToAction("Index");
            }

            Product productToDelete = db.Products
                .Where(p => p.pid == productId && p.uid == vendor.uid)
                .FirstOrDefault();

            if (productToDelete == null)
            {
                return RedirectToAction("Index");
            }

            db.Products.Remove(productToDelete);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}