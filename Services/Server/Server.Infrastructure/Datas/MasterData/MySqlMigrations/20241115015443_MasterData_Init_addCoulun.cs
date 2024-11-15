using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_addCoulun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudentCode",
                table: "TimeAttendenceEvent",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsLate",
                table: "TimeAttendenceEvent",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOffSoon",
                table: "TimeAttendenceEvent",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LateTime",
                table: "TimeAttendenceEvent",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OffSoonTime",
                table: "TimeAttendenceEvent",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLate",
                table: "TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "IsOffSoon",
                table: "TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "LateTime",
                table: "TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "OffSoonTime",
                table: "TimeAttendenceEvent");

            migrationBuilder.AlterColumn<string>(
                name: "StudentCode",
                table: "TimeAttendenceEvent",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
