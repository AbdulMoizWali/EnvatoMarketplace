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
            }
            return View(user);
        }
    }
}