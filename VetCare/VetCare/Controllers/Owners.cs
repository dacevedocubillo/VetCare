using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;
using Microsoft.AspNetCore.Authorization;

namespace VetCare.Controllers
{
    [Authorize]
    public class OwnersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OwnersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Owners
        public async Task<IActionResult> Index()
        {
            var owners = await _context.Owners
                .AsNoTracking()
                .ToListAsync();

            return View(owners);
        }

        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .Include(o => o.Pets)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return View(owner);
            }

            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "El propietario fue registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners.FindAsync(id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Owner owner)
        {
            if (id != owner.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(owner);
            }

            try
            {
                _context.Owners.Update(owner);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "El propietario fue actualizado correctamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OwnerExists(owner.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .Include(o => o.Pets)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _context.Owners.FindAsync(id);

            if (owner != null)
            {
                if (await _context.Pets.AnyAsync(p => p.OwnerId == id))
                {
                    TempData["ErrorMessage"] = "No se puede eliminar un propietario que tiene mascotas registradas.";
                    return RedirectToAction(nameof(Index));
                }
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "El propietario fue eliminado correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OwnerExists(int id)
        {
            return await _context.Owners
                .AnyAsync(o => o.Id == id);
        }
    }
}
