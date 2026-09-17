namespace VetCare.Services;
public interface IEmailService
{
    Task<bool> SendBookingConfirmationAsync(string email, string ownerName, string petName, DateTime scheduledAt);
    Task<bool> SendAppointmentReminderAsync(string email, string ownerName, string petName, DateTime scheduledAt);
}
