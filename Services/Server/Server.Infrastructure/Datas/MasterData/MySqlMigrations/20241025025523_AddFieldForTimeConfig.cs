using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class AddFieldForTimeConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EveningBreakTime",
                table: "A0_TimeConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EveningEndTime",
                table: "A0_TimeConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EveningLateTime",
                table: "A0_TimeConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EveningStartTime",
                table: "A0_TimeConfig",
                type: "time(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EveningBreakTime",
                table: "A0_TimeConfig");

            migrationBuilder.DropColumn(
                name: "EveningEndTime",
                table: "A0_TimeConfig");

            migrationBuilder.DropColumn(
                name: "EveningLateTime",
                table: "A0_TimeConfig");

            migrationBuilder.DropColumn(
                name: "EveningStartTime",
                table: "A0_TimeConfig");
        }
    }
}
