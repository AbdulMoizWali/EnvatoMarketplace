using EnvatoMarketplace.Models;
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
                LogUserLogin(log.uid);
                if (!string.IsNullOrEmpty(role))
                {
                    if (role == "Customer")
                    {
                        Session["uid"] = log.uid;
                        Session["username"] = username;
                        return RedirectToAction("Index", "Home");
                    }
                    else if (role == "Vendor")
                    {
                        Session["uid"] = log.uid;
                        Session["username"] = username;
                        return RedirectToAction("CreateUser", "Home");
                    }
                    else
                    {
                        Session["uid"] = log.uid;
                        Session["username"] = username;
                        return RedirectToAction("Index", "Admin");
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
        public ActionResult Signup(User user, string UserRole, string ConfirmPassword, string AcceptLicense)
        {
            if(user.password != ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords doesn't match");
                return View(user);
            }

            if(AcceptLicense != "Checked")
            {
                ModelState.AddModelError("", "To Sign up accept the privacy policy");
                return View(user);
            }


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
                    ModelState.AddModelError("", "Username name already taken unable to create account ");
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


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }


        [HttpPost]
        private void LogUserLogin(int userId)
        {
            Log logEntry = new Log();

            logEntry.uid = userId;
            logEntry.timestamp = DateTime.Now;
           

            db.Logs.Add(logEntry);
            db.SaveChanges();
        }

    }
}