using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Scheduleype",
                table: "ScheduleJob",
                newName: "ScheduleType");

            migrationBuilder.RenameColumn(
                name: "ScheduleTimeSend",
                table: "ScheduleJob",
                newName: "ScheduleTime");

            migrationBuilder.RenameColumn(
                name: "ScheduleSequentialSending",
                table: "ScheduleJob",
                newName: "ScheduleSequential");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduleType",
                table: "ScheduleJob",
                newName: "Scheduleype");

            migrationBuilder.RenameColumn(
                name: "ScheduleTime",
                table: "ScheduleJob",
                newName: "ScheduleTimeSend");

            migrationBuilder.RenameColumn(
                name: "ScheduleSequential",
                table: "ScheduleJob",
                newName: "ScheduleSequentialSending");
        }
    }
}
