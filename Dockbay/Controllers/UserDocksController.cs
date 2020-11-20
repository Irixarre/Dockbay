using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dockbay.Data;
using Dockbay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Dockbay.Controllers
{
    public class UserDocksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserDocksController> _logger;
        public UserDocksController(ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<UserDocksController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }
      

        // GET: UserDocks
        public async Task<IActionResult> Index(string disponible)
        {
            var applicationDbContext = _context.UserDock.Include(u => u.AppUser).Include(u => u.Dock);
            //return View(await applicationDbContext.ToListAsync());
            List<UserDock> userDocks = await _context.UserDock
                .Include(u => u.Dock)
                .Include(u => u.AppUser)
               //.FirstOrDefault(x => x.AppUserId == id)//Me llega vacio el id
                .ToListAsync();
            if (String.IsNullOrEmpty(disponible))
            {
                return View(userDocks);
            }
            if (disponible == "Yes")
            {
                userDocks = userDocks.Where(x => x.Exit == DateTime.MinValue).ToList();
                               
            }
            else 
            {
                userDocks = userDocks.Where(x => x.Exit != DateTime.MinValue).ToList();
            }
            return View(userDocks);


           
        }

        //*******************************Confirmacion rent
        public async Task<IActionResult> RentConfirmed(int? id)
        {
            Dock dock = await _context.Dock.FindAsync(id);
            //**************MIRAR ERROR!!!!!!!!!!!!
            //AppUser user = await _context.Users.FindAsync();
            UserDock rent = new UserDock
            {
                Dock = dock,
                //AppUser = user,
                Entrance = DateTime.Now,
                Exit = DateTime.MinValue,
            };
            _context.Add(rent);
            dock.Rented = true;
            _context.Update(dock);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Free(int id)
        {
            UserDock rent = await _context.UserDock.Include(x => x.Dock).FirstOrDefaultAsync(x => x.Id == id);
            return View(rent);
        }
        public async Task<IActionResult> ConfirmFree(int id)
        {
            UserDock rent = await _context.UserDock.Include(x => x.Dock).FirstOrDefaultAsync(x => x.Id == id);
            

            rent.Exit = DateTime.Now;
            Dock dock = rent.Dock;
            dock.Rented = false;
            _context.Update(dock);
            _context.Update(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Docks");
        }






        // GET: UserDocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDock = await _context.UserDock
                .Include(u => u.AppUser)
                .Include(u => u.Dock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDock == null)
            {
                return NotFound();
            }

            return View(userDock);
        }
        [Authorize(Roles = "admin")]

        // GET: UserDocks/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["DockId"] = new SelectList(_context.Set<Dock>(), "Id", "Town");
            return View();
        }
        [Authorize(Roles = "admin")]

        // POST: UserDocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Entrance,Exit,DockId,AppUserId")] UserDock userDock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userDock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Name", userDock.AppUserId);
            ViewData["DockId"] = new SelectList(_context.Set<Dock>(), "Id", "Town", userDock.DockId);
            return View(userDock);
        }
        [Authorize(Roles = "admin")]

        // GET: UserDocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDock = await _context.UserDock.FindAsync(id);
            if (userDock == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", userDock.AppUserId);
            ViewData["DockId"] = new SelectList(_context.Set<Dock>(), "Id", "Id", userDock.DockId);
            return View(userDock);
        }
        [Authorize(Roles = "admin")]

        // POST: UserDocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Entrance,Exit,DockId,AppUserId")] UserDock userDock)
        {
            if (id != userDock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDockExists(userDock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Name", userDock.AppUserId);
            ViewData["DockId"] = new SelectList(_context.Set<Dock>(), "Id", "Town", userDock.DockId);
            return View(userDock);
        }
        [Authorize(Roles = "admin")]

        // GET: UserDocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDock = await _context.UserDock
                .Include(u => u.AppUser)
                .Include(u => u.Dock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDock == null)
            {
                return NotFound();
            }

            return View(userDock);
        }
        [Authorize(Roles = "admin")]

        // POST: UserDocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userDock = await _context.UserDock.FindAsync(id);
            _context.UserDock.Remove(userDock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "admin")]

        private bool UserDockExists(int id)
        {
            return _context.UserDock.Any(e => e.Id == id);
        }

    }
}
