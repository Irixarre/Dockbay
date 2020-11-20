using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dockbay.Models
{
    public class UserDock
    {
        public int Id { get; set; }
        public DateTime Entrance { get; set; }
        public DateTime Exit { get; set; }
        public int DockId { get; set; }
        public string AppUserId { get; set; }
        public Dock Dock { get; set; }
        public AppUser AppUser { get; set; }
    }
}
