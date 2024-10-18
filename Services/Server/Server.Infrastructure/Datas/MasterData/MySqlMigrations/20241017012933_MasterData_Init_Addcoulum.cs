using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_Addcoulum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "GIO_VehicleInOut",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "GIO_VehicleInOut",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PlateColor",
                table: "GIO_VehicleInOut",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Speed",
                table: "GIO_VehicleInOut",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VehicleColor",
                table: "GIO_VehicleInOut",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "GIO_VehicleInOut",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direction",
                table: "GIO_VehicleInOut");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "GIO_VehicleInOut");

            migrationBuilder.DropColumn(
                name: "PlateColor",
                table: "GIO_VehicleInOut");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "GIO_VehicleInOut");

            migrationBuilder.DropColumn(
                name: "VehicleColor",
                table: "GIO_VehicleInOut");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "GIO_VehicleInOut");
        }
    }
}
