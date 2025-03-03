using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_wildfire_web.Migrations.WildfireDb
{
    /// <inheritdoc />
    public partial class FixFireIdAutoGeneration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop constraints (Foreign Keys & Primary Keys)
            migrationBuilder.DropForeignKey(name: "FK_UserFireSubscription_FireData", table: "UserFireSubscription");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND parent_object_id = OBJECT_ID('FireData'))
                BEGIN
                    ALTER TABLE FireData DROP CONSTRAINT PK__FireData__E1DECA144B6D0E6B;
                END
            ");

            // Create a backup table to prevent data loss
            migrationBuilder.Sql(@"
                SELECT * INTO FireData_Backup FROM FireData;
            ");

            // Create a temporary table with identical structure but FireId as IDENTITY
            migrationBuilder.Sql(@"
                CREATE TABLE FireData_Temp (
                    FireId INT IDENTITY(1,1) PRIMARY KEY,
                    Location GEOGRAPHY NOT NULL,
                    RadiativePower DECIMAL(10,2) NULL,
                    Polygon GEOGRAPHY NULL,
                    WeatherId INT NULL
                );
            ");

            // Copy existing data into the temporary table
            migrationBuilder.Sql(@"
                INSERT INTO FireData_Temp (Location, RadiativePower, Polygon, WeatherId)
                SELECT Location, RadiativePower, Polygon, WeatherId FROM FireData;
            ");

            // Drop the old FireData table **AFTER ensuring FireData_Temp has all the data**
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FireData')
                BEGIN
                    DROP TABLE FireData;
                END
            ");

            // Rename `FireData_Temp` to `FireData`
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FireData_Temp')
                BEGIN
                    EXEC sp_rename 'FireData_Temp', 'FireData';
                END
            ");

            // Re-add primary key explicitly after renaming
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND parent_object_id = OBJECT_ID('FireData'))
                BEGIN
                    ALTER TABLE FireData ADD CONSTRAINT PK__FireData__E1DECA144B6D0E6B PRIMARY KEY (FireId);
                END
            ");

            // Re-add foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_UserFireSubscription_FireData",
                table: "UserFireSubscription",
                column: "FireId",
                principalTable: "FireData",
                principalColumn: "FireId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_UserFireSubscription_FireData", table: "UserFireSubscription");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND parent_object_id = OBJECT_ID('FireData'))
                BEGIN
                    ALTER TABLE FireData DROP CONSTRAINT PK__FireData__E1DECA144B6D0E6B;
                END
            ");

            migrationBuilder.DropTable(name: "FireData");

            // Recreate old FireData table without Identity
            migrationBuilder.Sql(@"
                CREATE TABLE FireData (
                    FireId INT NOT NULL,
                    Location GEOGRAPHY NOT NULL,
                    RadiativePower DECIMAL(10,2) NULL,
                    Polygon GEOGRAPHY NULL,
                    WeatherId INT NULL,
                    CONSTRAINT PK__FireData__E1DECA144B6D0E6B PRIMARY KEY (FireId)
                );
            ");

            // Restore data from backup **ONLY IF IT EXISTS**
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FireData_Backup')
                BEGIN
                    INSERT INTO FireData (FireId, Location, RadiativePower, Polygon, WeatherId)
                    SELECT FireId, Location, RadiativePower, Polygon, WeatherId FROM FireData_Backup;
                END
            ");

            // Drop the backup table only **AFTER successful restore**
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FireData_Backup')
                BEGIN
                    DROP TABLE FireData_Backup;
                END
            ");

            // Re-add foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_UserFireSubscription_FireData",
                table: "UserFireSubscription",
                column: "FireId",
                principalTable: "FireData",
                principalColumn: "FireId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}