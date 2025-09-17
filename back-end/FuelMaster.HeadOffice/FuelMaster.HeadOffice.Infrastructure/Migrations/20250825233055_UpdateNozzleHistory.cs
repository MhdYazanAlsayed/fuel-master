using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMaster.HeadOffice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNozzleHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "NozzleHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TankId",
                table: "NozzleHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NozzleHistories_StationId",
                table: "NozzleHistories",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_NozzleHistories_TankId",
                table: "NozzleHistories",
                column: "TankId");

            migrationBuilder.AddForeignKey(
                name: "FK_NozzleHistories_Stations_StationId",
                table: "NozzleHistories",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NozzleHistories_Tanks_TankId",
                table: "NozzleHistories",
                column: "TankId",
                principalTable: "Tanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NozzleHistories_Stations_StationId",
                table: "NozzleHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_NozzleHistories_Tanks_TankId",
                table: "NozzleHistories");

            migrationBuilder.DropIndex(
                name: "IX_NozzleHistories_StationId",
                table: "NozzleHistories");

            migrationBuilder.DropIndex(
                name: "IX_NozzleHistories_TankId",
                table: "NozzleHistories");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "NozzleHistories");

            migrationBuilder.DropColumn(
                name: "TankId",
                table: "NozzleHistories");
        }
    }
}
