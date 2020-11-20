using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dockbay.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string BoatRegistration { get; set; }
        public List<UserDock> UserDockList { get; set; }
    }
}
