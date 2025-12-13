using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMaster.HeadOffice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFuelTypesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nozzles_FuelType_FuelTypeId",
                table: "Nozzles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tanks_FuelType_FuelTypeId",
                table: "Tanks");

            migrationBuilder.DropForeignKey(
                name: "FK_ZonePrices_FuelType_FuelTypeId",
                table: "ZonePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FuelType",
                table: "FuelType");

            migrationBuilder.RenameTable(
                name: "FuelType",
                newName: "FuelTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuelTypes",
                table: "FuelTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Nozzles_FuelTypes_FuelTypeId",
                table: "Nozzles",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tanks_FuelTypes_FuelTypeId",
                table: "Tanks",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ZonePrices_FuelTypes_FuelTypeId",
                table: "ZonePrices",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nozzles_FuelTypes_FuelTypeId",
                table: "Nozzles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tanks_FuelTypes_FuelTypeId",
                table: "Tanks");

            migrationBuilder.DropForeignKey(
                name: "FK_ZonePrices_FuelTypes_FuelTypeId",
                table: "ZonePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FuelTypes",
                table: "FuelTypes");

            migrationBuilder.RenameTable(
                name: "FuelTypes",
                newName: "FuelType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuelType",
                table: "FuelType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Nozzles_FuelType_FuelTypeId",
                table: "Nozzles",
                column: "FuelTypeId",
                principalTable: "FuelType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tanks_FuelType_FuelTypeId",
                table: "Tanks",
                column: "FuelTypeId",
                principalTable: "FuelType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZonePrices_FuelType_FuelTypeId",
                table: "ZonePrices",
                column: "FuelTypeId",
                principalTable: "FuelType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
