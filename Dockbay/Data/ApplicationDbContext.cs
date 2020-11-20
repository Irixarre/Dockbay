using System;
using System.Collections.Generic;
using System.Text;
using Dockbay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dockbay.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Dockbay.Models.UserDock> UserDock { get; set; }
        public DbSet<Dockbay.Models.Dock> Dock { get; set; }
    }
}
