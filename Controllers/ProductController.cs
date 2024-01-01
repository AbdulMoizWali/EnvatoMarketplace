using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        [HttpPost]
        public ActionResult Index(int? id, string Comment, string ReviewRating)
        {
            try
            {
                Product product = db.Products.First(prod => prod.pid == id);

                int uid = int.Parse(Session["uid"].ToString());
                int pid = (int)id;
                int reviewRating = int.Parse(ReviewRating.ToString());

                var reviewObj = new Review();

                reviewObj.uid = uid;
                reviewObj.pid = pid;
                reviewObj.comment = Comment;
                reviewObj.rating = reviewRating;


                if(product.Reviews.Count() > 0)
                {
                    double rating = 0;
                    foreach (var reviews in product.Reviews)
                    {
                        rating += (reviews.rating / (product.Reviews.Count() + 1));
                    }

                    rating += (reviewRating / (product.Reviews.Count() + 1));
                    product.rating = rating;
                }
                else
                {
                    product.rating = reviewRating;
                }

                db.Products.AddOrUpdate(product);
                db.Reviews.Add(reviewObj);
                db.SaveChanges();

                return RedirectToAction("Marketplace", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Default");
            }
        }
    }
}