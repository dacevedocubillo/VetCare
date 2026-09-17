using System.ComponentModel.DataAnnotations;

namespace VetCare.Models;

public class Appointment
{
    public int Id { get; set; }

    public DateTime ScheduledAt { get; set; }

    [Required]
    [StringLength(250)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Status { get; set; } = "Scheduled";

    [StringLength(500)]
    public string? Notes { get; set; }

    public bool ReminderSent { get; set; }

    public int PetId { get; set; }

    public Pet Pet { get; set; } = null!;
}
