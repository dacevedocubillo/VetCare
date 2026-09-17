using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCare.Models;

public class Pet
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Species { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Breed { get; set; }

    [StringLength(20)]
    public string? Sex { get; set; }

    public DateOnly? BirthDate { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal? Weight { get; set; }

    [StringLength(100)]
    public string? Color { get; set; }

    [StringLength(500)]
    public string? Allergies { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public int OwnerId { get; set; }

    public Owner Owner { get; set; } = null!;

    public ICollection<Appointment> Appointments { get; set; }
        = new List<Appointment>();

    public ICollection<MedicalRecord> MedicalRecords { get; set; }
        = new List<MedicalRecord>();
}