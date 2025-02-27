using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_wildfire_web.Migrations.WildfireDb
{
    /// <inheritdoc />
    public partial class UserUpdate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_UserFireSubscription_User",
                table: "UserFireSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocation_User",
                table: "UserLocation");

            // Drop primary keys
            migrationBuilder.DropPrimaryKey(
                name: "PK__User__1788CC4C8002BA03",
                table: "User");

            // Drop composite keys
            migrationBuilder.DropPrimaryKey(
                name: "PK__UserFire__499520EDB6A35324",
                table: "UserFireSubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK__UserLoca__79F72605BDDE839A",
                table: "UserLocation");

            // Drop AspNetIdentityUserId column
            migrationBuilder.DropColumn(
                name: "AspNetIdentityUserId",
                table: "User");

            // Change UserId from int to nvarchar(450)
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserFireSubscription",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserLocation",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Recreate primary keys
            migrationBuilder.AddPrimaryKey(
                name: "PK__User__1788CC4C8002BA03",
                table: "User",
                columns: ["UserId"]);

            // Recreate composite keys
            migrationBuilder.AddPrimaryKey(
                name: "PK__UserFire__499520EDB6A35324",
                table: "UserFireSubscription",
                columns: ["UserId", "FireId"]); // Composite key (UserId, FireId)

            migrationBuilder.AddPrimaryKey(
                name: "PK__UserLoca__79F72605BDDE839A",
                table: "UserLocation",
                columns: ["UserId", "LocationId"]); // Composite key (UserId, LocationId)

            // Recreate foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_UserFireSubscription_User",
                table: "UserFireSubscription",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocation_User",
                table: "UserLocation",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_UserFireSubscription_User",
                table: "UserFireSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocation_User",
                table: "UserLocation");

            // Drop primary keys
            migrationBuilder.DropPrimaryKey(
                name: "PK__User__1788CC4C8002BA03",
                table: "User");

            // Drop composite keys
            migrationBuilder.DropPrimaryKey(
                name: "PK__UserFire__499520EDB6A35324",
                table: "UserFireSubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK__UserLoca__79F72605BDDE839A",
                table: "UserLocation");

            // Revert UserId to int from nvarchar(450)
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "User",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserFireSubscription",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserLocation",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Add AspNetIdentityUserId column back
            migrationBuilder.AddColumn<string>(
                name: "AspNetIdentityUserId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true);

            // Recreate primary keys
            migrationBuilder.AddPrimaryKey(
                name: "PK__User__1788CC4C8002BA03",
                table: "User",
                columns: ["UserId"]);

            // Recreate composite keys
            migrationBuilder.AddPrimaryKey(
                name: "PK__UserFire__499520EDB6A35324",
                table: "UserFireSubscription",
                columns: ["UserId", "FireId"]); // Composite key (UserId, FireId)

            migrationBuilder.AddPrimaryKey(
                name: "PK__UserLoca__79F72605BDDE839A",
                table: "UserLocation",
                columns: ["UserId", "LocationId"]); // Composite key (UserId, LocationId)

            // Recreate foreign keys
            migrationBuilder.AddForeignKey(
                name: "FK_UserFireSubscription_User",
                table: "UserFireSubscription",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocation_User",
                table: "UserLocation",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}