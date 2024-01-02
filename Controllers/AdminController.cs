using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnvatoMarketplace.Models;
using EnvatoMarketplace.ViewModel;

namespace EnvatoMarketplace.Controllers
{
    public class AdminController : Controller
    {
        EnvatoDBEntities db = new EnvatoDBEntities();

        // GET: Admin
        public ActionResult Index()
        {
            getTotalCustomers();
            getTotalSales();
            getTotalProducts();

            // Retrieve data from session
            ViewBag.tCustomers = Session["tCustomers"] as int? ?? 0;
            ViewBag.tSales = Session["tSales"] as int? ?? 0;
            ViewBag.tProducts = Session["tProducts"] as int? ?? 0;

            List<User> users = db.Users.ToList();

            return View(users);
        }


        public ActionResult UserLogs()
        {
            List<User> users = db.Users.ToList();
            List<Log> logs = db.Logs.ToList();

            UserLogsVM viewModel = new UserLogsVM
            {
                Users = users,
                Logs = logs
            };

            return View(viewModel);
        }




        private void getTotalCustomers()
        {
            int totalCustomers = 0;
            if (ModelState.IsValid)
            {
                // customers role
                int roleIdToFilter = 3;
                totalCustomers = db.Users.Count(user => user.rid == roleIdToFilter);
            }
            Session["tCustomers"] = totalCustomers;
        }

        private void getTotalSales()
        {
            int totalSales = 0;
            if (ModelState.IsValid)
            {
                totalSales = db.Carts.Count(cart => cart.totalAmount != null);
            }
            Session["tSales"] = totalSales;
        }

        private void getTotalProducts()
        {
            int totalProducts = 0;
            if (ModelState.IsValid)
            {
                totalProducts = db.Products.Count(pd => pd.pid >= 0);
            }
            Session["tProducts"] = totalProducts;
        }
    }
}
