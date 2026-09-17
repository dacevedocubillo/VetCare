using Microsoft.EntityFrameworkCore;
using VetCare.Data;
namespace VetCare.Services;
public class AppointmentReminderService(IServiceScopeFactory scopeFactory, ILogger<AppointmentReminderService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var email = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var now = DateTime.Now; var limit = now.AddHours(24);
                var pending = await db.Appointments.Include(a => a.Pet).ThenInclude(p => p.Owner)
                    .Where(a => !a.ReminderSent && a.ScheduledAt > now && a.ScheduledAt <= limit && a.Status != "Cancelada").ToListAsync(stoppingToken);
                foreach (var appointment in pending)
                {
                    var owner = appointment.Pet.Owner;
                    if (!string.IsNullOrWhiteSpace(owner.Email) && await email.SendAppointmentReminderAsync(owner.Email, owner.FullName, appointment.Pet.Name, appointment.ScheduledAt)) appointment.ReminderSent = true;
                }
                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception exception) { logger.LogError(exception, "Error al procesar recordatorios de citas."); }
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
