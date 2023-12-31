using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnvatoMarketplace.Models
{
    public class MarketplaceVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}