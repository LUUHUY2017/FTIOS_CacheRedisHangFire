using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class AddLateTimeForConfigTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "BreakTime",
                table: "AttendanceTimeConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "LateTime",
                table: "AttendanceTimeConfig",
                type: "time(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakTime",
                table: "AttendanceTimeConfig");

            migrationBuilder.DropColumn(
                name: "LateTime",
                table: "AttendanceTimeConfig");
        }
    }
}
