using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnvatoMarketplace.Controllers
{
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

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

    }
    }