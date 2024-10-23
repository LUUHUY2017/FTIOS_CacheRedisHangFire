using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class AddAttendanceConfigTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "GIO_VehicleInOut",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "A0_AttendanceConfig",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MorningStartTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    MorningEndTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    MorningLateTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    MorningBreakTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    AfternoonStartTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    AfternoonEndTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    AfternoonLateTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    AfternoonBreakTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
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
                    table.PrimaryKey("PK_A0_AttendanceConfig", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "GIO_VehicleInOut");
        }
    }
}
