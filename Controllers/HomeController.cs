using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EnvatoMarketplace.Controllers
{
    public class HomeController : Controller
    {
        EnvatoDBEntities db = new EnvatoDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult CreateUser() {
            ViewBag.rid = new SelectList(db.Roles, "rid", "name");
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            if (ModelState.IsValid) {
                ViewBag.rid = new SelectList(db.Roles, "rid", "name", user.rid);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Categories() {
            return View(db.Categories.ToList());
        }

        public ActionResult CreateCategory()
        {
            ViewBag.pcatid = new SelectList(db.Categories, "catid", "catName");
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Categories");
                }
                catch (Exception ex)
                {
                    // Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.pcatid = new SelectList(db.Categories, "catid", "catName", category.pcatid);
            return View(category);
        }

    }
}