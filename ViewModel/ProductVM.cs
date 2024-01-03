using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnvatoMarketplace.ViewModel
{
    public class ProductVM
    {
        public Product NewProduct { get; set; }
        public SelectList Categories { get; set; }
        public bool NullifyQuantity { get; set; }
    }
}