using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartEMS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddInactiveReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InactiveReason",
                table: "Employees",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InactiveReason",
                table: "Employees");
        }
    }
}
