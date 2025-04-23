using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_wildfire_web.Migrations.FireDataDb
{
    /// <inheritdoc />
    public partial class AddIdToUserLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the Id column to UserLocations (assuming it should be an auto-incrementing column)
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserLocations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1"); // Assuming auto-increment is required
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the Id column from UserLocations
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserLocations");
        }
    }
}
