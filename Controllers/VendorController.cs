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

        public ActionResult AddProduct()
        {
            var categories = db.Categories.ToList();
            SelectList categoryList = new SelectList(categories, "catid", "catName");

            var viewModel = new ProductVM
            {
                NewProduct = new Product(),
                Categories = categoryList
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductVM viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.NewProduct.uid = Convert.ToInt32(Session["uid"]);
                viewModel.NewProduct.productPic = "~/uploads/productImages/galaxys21.jpeg";

                if (viewModel.NullifyQuantity)
                {
                    viewModel.NewProduct.availableQty = null;
                }
                try
                {
                    db.Products.Add(viewModel.NewProduct);
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException dbUpdateException)
                {
                    ModelState.AddModelError("", "Product name already taken ");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to Add Product  " + ex?.InnerException?.InnerException?.GetType()?.ToString() + " " + ex.Message);
                }

                return RedirectToAction("Index");
            }

            var categories = db.Categories.ToList(); 
            viewModel.Categories = new SelectList(categories, "catid", "catName");

            return View(viewModel);
        }

    }
}