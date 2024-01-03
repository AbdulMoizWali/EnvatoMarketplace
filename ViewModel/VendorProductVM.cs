using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EnvatoMarketplace.Models;
namespace EnvatoMarketplace.ViewModel
{
    public class VendorProductVM
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

}