using System.ComponentModel.DataAnnotations;
namespace VetCare.Models;
public class BookingViewModel
{
    [Required, StringLength(100)] public string OwnerName { get; set; } = string.Empty;
    [Required, Phone, StringLength(25)] public string Phone { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = string.Empty;
    [Required, StringLength(80)] public string PetName { get; set; } = string.Empty;
    [Required, StringLength(50)] public string Species { get; set; } = string.Empty;
    [StringLength(80)] public string? Breed { get; set; }
    [Required] public DateTime ScheduledAt { get; set; } = DateTime.Now.AddDays(1);
    [Required, StringLength(250)] public string Reason { get; set; } = string.Empty;
}
