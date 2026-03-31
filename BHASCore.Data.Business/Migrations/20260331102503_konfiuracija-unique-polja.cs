using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BHASCore.Data.Business.Migrations
{
    /// <inheritdoc />
    public partial class konfiuracijauniquepolja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JMBG",
                table: "Employees",
                column: "JMBG",
                unique: true,
                filter: "[JMBG] IS NOT NULL");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_Email",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_JMBG",
                table: "Employees");
        }
    }
}
