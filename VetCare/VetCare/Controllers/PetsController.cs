using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;
using Microsoft.AspNetCore.Authorization;

namespace VetCare.Controllers;

[Authorize]
public class PetsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index() => View(await context.Pets.Include(p => p.Owner).AsNoTracking().OrderBy(p => p.Name).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var pet = await context.Pets.Include(p => p.Owner).Include(p => p.Appointments).Include(p => p.MedicalRecords).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return pet is null ? NotFound() : View(pet);
    }

    public async Task<IActionResult> Create(int? ownerId)
    {
        await LoadOwners(ownerId);
        return View(new Pet { OwnerId = ownerId ?? 0 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pet pet)
    {
        if (!await context.Owners.AnyAsync(o => o.Id == pet.OwnerId)) ModelState.AddModelError(nameof(Pet.OwnerId), "Seleccione un propietario válido.");
        if (!ModelState.IsValid) { await LoadOwners(pet.OwnerId); return View(pet); }
        context.Add(pet); await context.SaveChangesAsync();
        TempData["SuccessMessage"] = "La mascota fue registrada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var pet = await context.Pets.FindAsync(id);
        if (pet is null) return NotFound();
        await LoadOwners(pet.OwnerId); return View(pet);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Pet pet)
    {
        if (id != pet.Id) return NotFound();
        if (!ModelState.IsValid) { await LoadOwners(pet.OwnerId); return View(pet); }
        context.Update(pet); await context.SaveChangesAsync();
        TempData["SuccessMessage"] = "La mascota fue actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var pet = await context.Pets.Include(p => p.Owner).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return pet is null ? NotFound() : View(pet);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pet = await context.Pets.FindAsync(id);
        if (pet is not null) { context.Remove(pet); await context.SaveChangesAsync(); TempData["SuccessMessage"] = "La mascota y sus datos asociados fueron eliminados."; }
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadOwners(int? selected = null) => ViewBag.OwnerId = new SelectList(await context.Owners.Where(o => o.IsActive).OrderBy(o => o.FullName).ToListAsync(), "Id", "FullName", selected);
}
