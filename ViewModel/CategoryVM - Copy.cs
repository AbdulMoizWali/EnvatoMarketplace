using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnvatoMarketplace.ViewModel
{
    // ViewModel
    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; }
        public SelectList ParentCategories { get; set; }
        public Category NewCategory { get; set; }
    }

}