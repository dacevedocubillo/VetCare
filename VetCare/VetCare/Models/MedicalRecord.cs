using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCare.Models;

public class MedicalRecord
{
    public int Id { get; set; }

    public DateTime ConsultationDate { get; set; } = DateTime.UtcNow;

    [StringLength(1000)]
    public string? Symptoms { get; set; }

    [Required]
    [StringLength(1000)]
    public string Diagnosis { get; set; } = string.Empty;

    [StringLength(1500)]
    public string? Treatment { get; set; }

    [StringLength(1500)]
    public string? Notes { get; set; }

    [Column(TypeName = "decimal(6,2)")]
    public decimal? RecordedWeight { get; set; }

    public DateTime? NextReviewDate { get; set; }

    public int PetId { get; set; }

    public Pet Pet { get; set; } = null!;
}