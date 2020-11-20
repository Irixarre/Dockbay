using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dockbay.Data;
using Dockbay.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dockbay.Controllers
{
    public class DocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Docks
        public async Task<IActionResult> Index(string town, string disponible)
        {
            List<Dock> docks = await _context.Dock.OrderBy(p => p.Town).ToListAsync();
            ViewData["disponibles"] = docks.Select(g => g.Disponible).Distinct().ToList();

            if (String.IsNullOrEmpty(town) && String.IsNullOrEmpty(disponible))
            {
                return View(docks);
            }
            if (!String.IsNullOrEmpty(town))
            {
                docks = docks.Where(x => x.Town.ToLower()
                               .Contains(town.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(disponible))
            {
                docks = docks.Where(x => x.Disponible == disponible).ToList();
            }
            return View(docks);
        }
        //codigo amarre
        public async Task<IActionResult> Rent(int id)
        {
            Dock dock = await _context.Dock.FindAsync(id);

            return View(dock);
        }
        



        // GET: Docks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dock = await _context.Dock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dock == null)
            {
                return NotFound();
            }

            return View(dock);
        }
        [Authorize(Roles = "admin")]

        // GET: Docks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Docks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Town,History,Coordinates,Image,Rented")] Dock dock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dock);
        }
        [Authorize(Roles = "admin")]

        // GET: Docks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dock = await _context.Dock.FindAsync(id);
            if (dock == null)
            {
                return NotFound();
            }
            return View(dock);
        }
        [Authorize(Roles = "admin")]

        // POST: Docks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Town,History,Coordinates,Image,Rented")] Dock dock)
        {
            if (id != dock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DockExists(dock.Id))
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
            return View(dock);
        }
        [Authorize(Roles = "admin")]



        // GET: Docks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dock = await _context.Dock
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dock == null)
            {
                return NotFound();
            }

            return View(dock);
        }
        [Authorize(Roles = "admin")]

        // POST: Docks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dock = await _context.Dock.FindAsync(id);
            _context.Dock.Remove(dock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "admin")]

        private bool DockExists(int id)
        {
            return _context.Dock.Any(e => e.Id == id);
        }
    }
}
