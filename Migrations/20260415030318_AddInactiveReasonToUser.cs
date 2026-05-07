using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartEMS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddInactiveReasonToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InactiveReason",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "InactiveReason",
                table: "Users",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InactiveReason",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "InactiveReason",
                table: "Employees",
                type: "longtext",
                nullable: true);
        }
    }
}
