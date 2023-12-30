﻿using EnvatoMarketplace.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.Common;
using System.Web.WebPages;

namespace EnvatoMarketplace.Controllers
{
    enum UserTypes {
        Admin=1,
        Vendor=2,
        Customer=3,            
    }

    enum UserAccountStatus
    {
        TRUE,
        FALSE,
    }

    public class AuthController : Controller
    {


        EnvatoDBEntities db = new EnvatoDBEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string username, string password)
        {
            var log = db.Users.Where(x => x.username == username && x.password == password).FirstOrDefault();

            if (log != null)
            {
                string role = log.Role.name;

                if (!string.IsNullOrEmpty(role))
                {
                    if (role == "Customer")
                    {
                        Session["username"] = username;
                        return RedirectToAction("Index", "Home");
                    }
                    else if (role == "Vendor")
                    {
                        Session["username"] = username;
                        return RedirectToAction("CreateUser", "Home");
                    }
                    else
                    {
                        return RedirectToAction("CreateCategory", "Home");
                    }
                }
            }

           
            return View();
        }

        private UserTypes getUserRole(string role)
        {
            if (role == "Buyer")
            {
                return UserTypes.Customer;
            }
            else if (role == "Seller")
            {
                return UserTypes.Vendor;
            }
            else
            {
                return UserTypes.Admin;
            }
        }

        //Signup as Customer

        public ActionResult Signup() {
            /*ViewBag.rid = new SelectList(db.Roles, "rid", "name");*/
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User user, string UserRole)
        {


            user.rid = (int)getUserRole(UserRole);

            user.profilePic = "~/uploads/userImages/user.png";
            if (user.doj == null)
            {
                user.doj = DateTime.Now;  // Set to current date and time
            }
            if (user.accountStatus == null)
            {
                user.accountStatus = UserAccountStatus.TRUE.ToString();  // Set to "TRUE"
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }

                catch (System.Data.Entity.Infrastructure.DbUpdateException dbUpdateException)
                {
                    ModelState.AddModelError("", "Username name already taken unable to signup ");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to signup  " + ex?.InnerException?.InnerException?.GetType()?.ToString() + " " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "Unable to signup state " + UserRole);
            }
            return View(user);
        }

        
    }
}