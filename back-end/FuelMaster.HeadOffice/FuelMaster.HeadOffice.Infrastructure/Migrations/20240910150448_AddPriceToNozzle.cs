using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMaster.HeadOffice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToNozzle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Nozzles",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Nozzles");
        }
    }
}
