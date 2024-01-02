using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
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


                if(product.Reviews.Count > 0)
                {
                    int ratingSum = 0;
                    foreach (var reviews in product.Reviews)
                    {
                        ratingSum += reviews.rating;
                        /*rating += (double)((double)reviews.rating / (product.Reviews.Count + 1));
                        ModelState.AddModelError("", rating.ToString());
                        Debug.WriteLine(rating.ToString() + " " + reviews.rating + " / " + (product.Reviews.Count + 1));  */ 
                    }
                    ratingSum += reviewRating;

                    double rating = 0.0;
                    rating = (ratingSum / (product.Reviews.Count + 1));
                    Debug.WriteLine(rating.ToString() + " " + ratingSum + " / " + (product.Reviews.Count + 1));
                    product.rating = rating;
                }
                else
                {
                    product.rating = reviewRating;
                }

                ModelState.AddModelError("", product.Reviews.Count.ToString());

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