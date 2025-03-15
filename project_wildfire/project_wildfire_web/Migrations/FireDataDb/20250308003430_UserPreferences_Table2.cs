using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_wildfire_web.Migrations.FireDataDb
{
    /// <inheritdoc />
    public partial class UserPreferences_Table2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name:"PK_UserPreferences", table: "UserPreferences");
            migrationBuilder.AddForeignKey(name:"FK_UserPreferences_Users", table: "UserPreferences", column:"UserId", principalTable: "Users", principalColumn: "UserId", onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name:"FK_UserPreferences_Users", table:"UserPreferences");
            migrationBuilder.AddPrimaryKey(name:"PK_UserPreferences", table: "UserPreferences", column:"UserId");
        }
    }
}
