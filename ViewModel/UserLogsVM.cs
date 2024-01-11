using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnvatoMarketplace.ViewModel
{
    public class UserLogsVM
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Log> Logs { get; set; }
    }
}