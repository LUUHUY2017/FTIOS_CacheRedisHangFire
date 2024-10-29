using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_addAttendenceDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AbsenceDate",
                table: "TA_TimeAttendenceEvent",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttendenceSection",
                table: "TA_TimeAttendenceEvent",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassCode",
                table: "TA_TimeAttendenceEvent",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "FormSendSMS",
                table: "TA_TimeAttendenceEvent",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolCode",
                table: "TA_TimeAttendenceEvent",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SchoolYearCode",
                table: "TA_TimeAttendenceEvent",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StudentCode",
                table: "TA_TimeAttendenceEvent",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ValueAbSent",
                table: "TA_TimeAttendenceEvent",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "SynStatus",
                table: "A2_PersonSynToDevice",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TA_TimeAttendenceDetail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TA_TimeAttendenceEventId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsLate = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    IsOffSoon = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    IsOffPeriod = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    LateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OffSoonTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PeriodI = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PeriodII = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PeriodIII = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PeriodIV = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PeriodV = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PeriodVI = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    AbsenceTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
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
                    OrganizationId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReferenceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TA_TimeAttendenceDetail", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TA_TimeAttendenceDetail");

            migrationBuilder.DropColumn(
                name: "AbsenceDate",
                table: "TA_TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "AttendenceSection",
                table: "TA_TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "ClassCode",
                table: "TA_TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "FormSendSMS",
                table: "TA_TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "SchoolCode",
                table: "TA_TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "SchoolYearCode",
                table: "TA_TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "StudentCode",
                table: "TA_TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "ValueAbSent",
                table: "TA_TimeAttendenceEvent");

            migrationBuilder.DropColumn(
                name: "SynStatus",
                table: "A2_PersonSynToDevice");
        }
    }
}
