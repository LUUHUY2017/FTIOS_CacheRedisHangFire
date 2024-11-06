using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_addA0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "access_token",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "expires_in",
                table: "A0_AttendanceConfig",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "time_expires_in",
                table: "A0_AttendanceConfig",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "token_type",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "access_token",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "expires_in",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "refresh_token",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "time_expires_in",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "token_type",
                table: "A0_AttendanceConfig");
        }
    }
}
