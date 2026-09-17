using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;
using Microsoft.AspNetCore.Authorization;

namespace VetCare.Controllers;

[Authorize]
public class MedicalRecordsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index() => View(await context.MedicalRecords.Include(r => r.Pet).ThenInclude(p => p.Owner).AsNoTracking().OrderByDescending(r => r.ConsultationDate).ToListAsync());
    public async Task<IActionResult> Create(int? petId) { await LoadPets(petId); return View(new MedicalRecord { PetId = petId ?? 0, ConsultationDate = DateTime.Now }); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MedicalRecord record) { if (!ModelState.IsValid) { await LoadPets(record.PetId); return View(record); } context.Add(record); await context.SaveChangesAsync(); TempData["SuccessMessage"] = "El historial médico fue registrado."; return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> Edit(int? id) { if (id is null) return NotFound(); var item = await context.MedicalRecords.FindAsync(id); if (item is null) return NotFound(); await LoadPets(item.PetId); return View(item); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MedicalRecord record) { if (id != record.Id) return NotFound(); if (!ModelState.IsValid) { await LoadPets(record.PetId); return View(record); } context.Update(record); await context.SaveChangesAsync(); TempData["SuccessMessage"] = "El historial médico fue actualizado."; return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id) { var item = await context.MedicalRecords.FindAsync(id); if (item is not null) { context.Remove(item); await context.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }
    private async Task LoadPets(int? selected = null) => ViewBag.PetId = new SelectList(await context.Pets.Include(p => p.Owner).Where(p => p.IsActive).OrderBy(p => p.Name).Select(p => new { p.Id, Name = p.Name + " — " + p.Owner.FullName }).ToListAsync(), "Id", "Name", selected);
}
