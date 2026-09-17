using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;
using Microsoft.AspNetCore.Authorization;

namespace VetCare.Controllers;

[Authorize]
public class AppointmentsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index() => View(await context.Appointments.Include(a => a.Pet).ThenInclude(p => p.Owner).AsNoTracking().OrderBy(a => a.ScheduledAt).ToListAsync());
    public async Task<IActionResult> Create(int? petId) { await LoadPets(petId); return View(new Appointment { PetId = petId ?? 0, ScheduledAt = DateTime.Now.AddHours(1) }); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        if (!ModelState.IsValid) { await LoadPets(appointment.PetId); return View(appointment); }
        context.Add(appointment); await context.SaveChangesAsync(); TempData["SuccessMessage"] = "La cita fue agendada correctamente."; return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound(); var item = await context.Appointments.FindAsync(id); if (item is null) return NotFound(); await LoadPets(item.PetId); return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Appointment appointment)
    {
        if (id != appointment.Id) return NotFound(); if (!ModelState.IsValid) { await LoadPets(appointment.PetId); return View(appointment); }
        context.Update(appointment); await context.SaveChangesAsync(); TempData["SuccessMessage"] = "La cita fue actualizada."; return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id) { var item = await context.Appointments.FindAsync(id); if (item is not null) { context.Remove(item); await context.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }

    private async Task LoadPets(int? selected = null) => ViewBag.PetId = new SelectList(await context.Pets.Include(p => p.Owner).Where(p => p.IsActive).OrderBy(p => p.Name).Select(p => new { p.Id, Name = p.Name + " — " + p.Owner.FullName }).ToListAsync(), "Id", "Name", selected);
}
