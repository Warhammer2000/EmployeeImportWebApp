using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeImportApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEditingToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEditing",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditing",
                table: "Employees");
        }
    }
}
