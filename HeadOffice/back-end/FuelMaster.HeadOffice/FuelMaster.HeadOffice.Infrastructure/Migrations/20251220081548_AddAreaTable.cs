using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMaster.HeadOffice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Area_AreaId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Area_AreaId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_CityId",
                table: "Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Area",
                table: "Area");

            migrationBuilder.RenameTable(
                name: "Area",
                newName: "Areas");

            migrationBuilder.AddColumn<int>(
                name: "StationCount",
                table: "Areas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Areas",
                table: "Areas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_CityId_Id",
                table: "Stations",
                columns: new[] { "CityId", "Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Areas_AreaId",
                table: "Employees",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Areas_AreaId",
                table: "Stations",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Areas_AreaId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Areas_AreaId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_CityId_Id",
                table: "Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Areas",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "StationCount",
                table: "Areas");

            migrationBuilder.RenameTable(
                name: "Areas",
                newName: "Area");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Area",
                table: "Area",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_CityId",
                table: "Stations",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Area_AreaId",
                table: "Employees",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Area_AreaId",
                table: "Stations",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
