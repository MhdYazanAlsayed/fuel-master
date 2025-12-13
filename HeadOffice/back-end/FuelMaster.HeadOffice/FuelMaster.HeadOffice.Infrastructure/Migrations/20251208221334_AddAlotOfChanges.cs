using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuelMaster.HeadOffice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlotOfChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FuelMasterGroup_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "FuelMasterGroup");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "AspNetUsers",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_RoleId");

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Scope",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelMasterAreasOfAccess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaOfAccess = table.Column<int>(type: "int", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EnglishDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ArabicDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelMasterAreasOfAccess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelMasterRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelMasterRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelMasterPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    AreaOfAccessId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelMasterPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelMasterPermissions_FuelMasterAreasOfAccess_AreaOfAccessId",
                        column: x => x.AreaOfAccessId,
                        principalTable: "FuelMasterAreasOfAccess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FuelMasterPermissions_FuelMasterRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "FuelMasterRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_AreaId_Id",
                table: "Stations",
                columns: new[] { "AreaId", "Id" },
                unique: true,
                filter: "[AreaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AreaId",
                table: "Employees",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CityId",
                table: "Employees",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelMasterPermissions_AreaOfAccessId",
                table: "FuelMasterPermissions",
                column: "AreaOfAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelMasterPermissions_RoleId",
                table: "FuelMasterPermissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FuelMasterRoles_RoleId",
                table: "AspNetUsers",
                column: "RoleId",
                principalTable: "FuelMasterRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Area_AreaId",
                table: "Employees",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Cities_CityId",
                table: "Employees",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_FuelMasterRoles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "FuelMasterRoles",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FuelMasterRoles_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Area_AreaId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Cities_CityId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_FuelMasterRoles_RoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Area_AreaId",
                table: "Stations");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "FuelMasterPermissions");

            migrationBuilder.DropTable(
                name: "FuelMasterAreasOfAccess");

            migrationBuilder.DropTable(
                name: "FuelMasterRoles");

            migrationBuilder.DropIndex(
                name: "IX_Stations_AreaId_Id",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AreaId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Scope",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "AspNetUsers",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_GroupId");

            migrationBuilder.CreateTable(
                name: "FuelMasterGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelMasterGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuelMasterGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_FuelMasterGroup_FuelMasterGroupId",
                        column: x => x.FuelMasterGroupId,
                        principalTable: "FuelMasterGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_FuelMasterGroupId",
                table: "Permission",
                column: "FuelMasterGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FuelMasterGroup_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "FuelMasterGroup",
                principalColumn: "Id");
        }
    }
}
