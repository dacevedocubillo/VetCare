using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
namespace VetCare.Services;
public class SmtpEmailService(IOptions<SmtpSettings> options, ILogger<SmtpEmailService> logger) : IEmailService
{
    private readonly SmtpSettings settings = options.Value;
    public Task<bool> SendBookingConfirmationAsync(string email, string ownerName, string petName, DateTime scheduledAt) =>
        SendAsync(email, "Solicitud de cita recibida", $"Hola {ownerName}, recibimos la cita de {petName} para el {scheduledAt:dd/MM/yyyy 'a las' HH:mm}. La clínica revisará la solicitud.");
    public Task<bool> SendAppointmentReminderAsync(string email, string ownerName, string petName, DateTime scheduledAt) =>
        SendAsync(email, "Recordatorio de cita veterinaria", $"Hola {ownerName}, le recordamos que {petName} tiene una cita el {scheduledAt:dd/MM/yyyy 'a las' HH:mm}.");
    private async Task<bool> SendAsync(string recipient, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(settings.Host) || string.IsNullOrWhiteSpace(settings.FromEmail)) { logger.LogWarning("Correo omitido: SMTP no está configurado."); return false; }
        try
        {
            using var message = new MailMessage { From = new MailAddress(settings.FromEmail, settings.FromName), Subject = subject, Body = body, IsBodyHtml = false };
            message.To.Add(recipient);
            using var client = new SmtpClient(settings.Host, settings.Port) { EnableSsl = settings.EnableSsl, Credentials = new NetworkCredential(settings.UserName, settings.Password) };
            await client.SendMailAsync(message); return true;
        }
        catch (Exception exception) { logger.LogError(exception, "No se pudo enviar correo a {Recipient}", recipient); return false; }
    }
}
