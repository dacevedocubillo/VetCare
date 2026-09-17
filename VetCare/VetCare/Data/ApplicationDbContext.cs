using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VetCare.Models;

namespace VetCare.Data;

public class ApplicationDbContext
    : IdentityDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Owner>()
            .HasMany(owner => owner.Pets)
            .WithOne(pet => pet.Owner)
            .HasForeignKey(pet => pet.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Pet>()
            .HasMany(pet => pet.Appointments)
            .WithOne(appointment => appointment.Pet)
            .HasForeignKey(appointment => appointment.PetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Pet>()
            .HasMany(pet => pet.MedicalRecords)
            .WithOne(record => record.Pet)
            .HasForeignKey(record => record.PetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}