using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Hubs;
using VetCare.Models;
using VetCare.Services;
namespace VetCare.Controllers;
[AllowAnonymous]
[Route("reservar")]
public class BookingController(ApplicationDbContext context, IHubContext<AppointmentsHub> hub, IEmailService emailService) : Controller
{
    [HttpGet("")]
    public IActionResult Create() => View(new BookingViewModel());

    [HttpPost(""), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingViewModel model)
    {
        if (model.ScheduledAt <= DateTime.Now) ModelState.AddModelError(nameof(model.ScheduledAt), "Seleccione una fecha futura.");
        if (!ModelState.IsValid) return View(model);

        await using var transaction = await context.Database.BeginTransactionAsync();
        var owner = await context.Owners.FirstOrDefaultAsync(o => o.Email == model.Email);
        if (owner is null)
        {
            owner = new Owner { FullName = model.OwnerName, Phone = model.Phone, Email = model.Email };
            context.Owners.Add(owner); await context.SaveChangesAsync();
        }
        else { owner.FullName = model.OwnerName; owner.Phone = model.Phone; }

        var pet = await context.Pets.FirstOrDefaultAsync(p => p.OwnerId == owner.Id && p.Name == model.PetName);
        if (pet is null)
        {
            pet = new Pet { OwnerId = owner.Id, Name = model.PetName, Species = model.Species, Breed = model.Breed };
            context.Pets.Add(pet); await context.SaveChangesAsync();
        }

        var appointment = new Appointment { PetId = pet.Id, ScheduledAt = model.ScheduledAt, Reason = model.Reason, Status = "Solicitada" };
        context.Appointments.Add(appointment); await context.SaveChangesAsync(); await transaction.CommitAsync();

        await hub.Clients.All.SendAsync("AppointmentCreated", new { appointment.Id, Pet = pet.Name, Owner = owner.FullName, appointment.ScheduledAt, appointment.Reason });
        await emailService.SendBookingConfirmationAsync(owner.Email!, owner.FullName, pet.Name, appointment.ScheduledAt);
        return RedirectToAction(nameof(Success));
    }

    [HttpGet("confirmacion")]
    public IActionResult Success() => View();
}
