using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
namespace VetCare.Data.Migrations;
[DbContext(typeof(ApplicationDbContext))]
[Migration("20260818010000_AddAppointmentReminder")]
public class AddAppointmentReminder : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<bool>(name: "ReminderSent", table: "Appointments", type: "bit", nullable: false, defaultValue: false);
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(name: "ReminderSent", table: "Appointments");
}
