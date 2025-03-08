using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_wildfire_web.Migrations
{
    /// <inheritdoc />
    public partial class AddSavedLocationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name:"UserId", table: "UserPreferences");
            migrationBuilder.AddForeignKey(name:"FK_UserPreferences_Users", table: "UserPreferences", column:"UserId", principalTable: "Users", principalColumn: "UserId", onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name:"UserId", table:"UserPreferences");
            migrationBuilder.AddPrimaryKey(name:"UserId", table: "UserPreferences", column:"UserId");
        }
    }
}
