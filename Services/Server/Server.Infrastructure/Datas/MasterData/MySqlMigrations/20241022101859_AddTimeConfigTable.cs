using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class AddTimeConfigTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfternoonBreakTime",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "AfternoonEndTime",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "AfternoonLateTime",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "AfternoonStartTime",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "MorningBreakTime",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "MorningEndTime",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "MorningLateTime",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "MorningStartTime",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "A0_AttendanceConfig");

            migrationBuilder.CreateTable(
                name: "A0_TimeConfig",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrganizationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MorningStartTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    MorningEndTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    MorningLateTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    MorningBreakTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    AfternoonStartTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    AfternoonEndTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    AfternoonLateTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    AfternoonBreakTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Actived = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Reason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReferenceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_A0_TimeConfig", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "A0_TimeConfig");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AfternoonBreakTime",
                table: "A0_AttendanceConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AfternoonEndTime",
                table: "A0_AttendanceConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AfternoonLateTime",
                table: "A0_AttendanceConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AfternoonStartTime",
                table: "A0_AttendanceConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MorningBreakTime",
                table: "A0_AttendanceConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MorningEndTime",
                table: "A0_AttendanceConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MorningLateTime",
                table: "A0_AttendanceConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MorningStartTime",
                table: "A0_AttendanceConfig",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
