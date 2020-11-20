using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dockbay.Models
{
    public class Dock
    {
        public int Id { get; set; }
        public string Town { get; set; }
        public string History { get; set; }
        public string Coordinates { get; set; }
        
        public string Image { get; set; }
        public bool Rented { get; set; }
        public string Disponible { get; set; }
        public List<UserDock> UserDockList { get; set; }
    }
}
