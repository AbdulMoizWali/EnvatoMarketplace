using EnvatoMarketplace.Models;
using EnvatoMarketplace.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
            DeleteReviewsForProduct(productToDelete.pid);
            DeleteCartItemsForProduct(productToDelete.pid);
            db.Products.Remove(productToDelete);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        private void DeleteReviewsForProduct(int productId)
        {
            var reviewsToDelete = db.Reviews.Where(r => r.pid == productId).ToList();

            if (reviewsToDelete.Count > 0)
            {
                db.Reviews.RemoveRange(reviewsToDelete);
                db.SaveChanges();
            }
        }

        private void DeleteCartItemsForProduct(int productId)
        {
            var cartItemsToDelete = db.CartItems.Where(ci => ci.pid == productId).ToList();

            if (cartItemsToDelete.Count > 0)
            {
                db.CartItems.RemoveRange(cartItemsToDelete);
                db.SaveChanges();
            }
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
        public ActionResult AddProduct(ProductVM viewModel, HttpPostedFileBase imgFile) 
        {
            if (ModelState.IsValid && imgFile != null && imgFile.ContentLength > 0)
            {
               

                    string fileName = Path.GetFileName(imgFile.FileName);
                    String _fileName = fileName;
                    String extension = Path.GetExtension(imgFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/uploads/productImages/"), fileName);
                    viewModel.NewProduct.productPic = "~/uploads/productImages/" + _fileName;

                    viewModel.NewProduct.uid = Convert.ToInt32(Session["uid"]);
                   

                    if (viewModel.NullifyQuantity)
                    {
                        viewModel.NewProduct.availableQty = null;
                    }
                    try
                    {
                        db.Products.Add(viewModel.NewProduct);
                    imgFile.SaveAs(path);
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
            else
            {
                return RedirectToAction("InternalServerError", "Default");
            }

            var categories = db.Categories.ToList(); 
            viewModel.Categories = new SelectList(categories, "catid", "catName");

            return View(viewModel);
        }

    }
}