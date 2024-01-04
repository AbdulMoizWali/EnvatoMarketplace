using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnvatoMarketplace.Controllers
{
    public class UserController : Controller
    {

        EnvatoDBEntities db = new EnvatoDBEntities();
        // GET: User
        public ActionResult Index(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if(id == null)
            {
                id = int.Parse(Session["uid"].ToString());
            }
            string username = Session["username"].ToString();
            var user = db.Users.Where(usr => usr.uid == id && usr.username == username).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        public ActionResult Index(User user, HttpPostedFileBase ProfilePic) {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(ProfilePic.FileName);
                var path = Path.Combine(Server.MapPath("~/upload/userImages"), fileName);

                var prevUser = db.Users.Where(usr => usr.uid == user.uid).FirstOrDefault();

                user.doj = prevUser.doj;
                user.username = prevUser.username;

                user.profilePic = "~/upload/userImages/" + fileName;

                db.Users.AddOrUpdate(user);

                Debug.WriteLine(ProfilePic.FileName);
                Debug.WriteLine(user.uid);
                Debug.WriteLine(user.name);
                Debug.WriteLine(user.username);
                Debug.WriteLine(user.password);
                Debug.WriteLine(user.profilePic);
                Debug.WriteLine(user.dob);
                Debug.WriteLine(user.doj);
                Debug.WriteLine(user.shippingAddress);
                Debug.WriteLine(user.phone);

                if (db.SaveChanges() > 0)
                {
                    ProfilePic.SaveAs(path);
                }

                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}