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
        public ActionResult Index(User user, HttpPostedFileBase profilePic, string dob) {
            var prevUser = db.Users.Where(usr => usr.uid == user.uid).FirstOrDefault();

            var fileName = "";

            if (profilePic != null)
            {
                fileName = Path.GetFileName(profilePic.FileName);
            }
            else
            {
                fileName = prevUser.profilePic.Split('/')[prevUser.profilePic.Split('/').Count() - 1];
            }
            
            var path = Path.Combine(Server.MapPath("~/uploads/userImages/"), fileName);

            if(user.accountStatus == null)
            {
                user.accountStatus = UserAccountStatus.TRUE.ToString();
            }
            user.doj = prevUser.doj;
            user.username = prevUser.username;
            user.rid = prevUser.rid;
            user.profilePic = "~/uploads/userImages/" + fileName;
            if(dob != "")
            {
                user.dob = DateTime.Parse(dob);
            }
            else
            {
                user.dob = prevUser.dob;
            }

            db.Users.AddOrUpdate(user);

            Debug.WriteLine(path);
            Debug.WriteLine(fileName);
            Debug.WriteLine(profilePic?.FileName);
            Debug.WriteLine(user.uid);
            Debug.WriteLine(user.name);
            Debug.WriteLine(user.username);
            Debug.WriteLine(user.password);
            Debug.WriteLine(user.profilePic);
            Debug.WriteLine(user.dob);
            Debug.WriteLine(user.doj);
            Debug.WriteLine(user.shippingAddress);
            Debug.WriteLine(user.phone);

            int changedlines = db.SaveChanges();
            Debug.WriteLine(changedlines);

            if (profilePic != null)
            {
                profilePic.SaveAs(path);

            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
   
            return View(user);
        }
    }
}