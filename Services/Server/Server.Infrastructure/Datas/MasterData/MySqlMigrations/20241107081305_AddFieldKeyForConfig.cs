using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class AddFieldKeyForConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KeyIV",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "KeyIV",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "A0_AttendanceConfig");
        }
    }
}
