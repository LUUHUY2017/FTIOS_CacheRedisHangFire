using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_indegr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeAttendenceEvent_CreatedDate",
                table: "TimeAttendenceEvent");

            migrationBuilder.CreateIndex(
                name: "IX_Org_Eventtype_Enroll",
                table: "TimeAttendenceEvent",
                columns: new[] { "OrganizationId", "EventTime", "EnrollNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Org_Eventtype_Eventtime",
                table: "TimeAttendenceEvent",
                columns: new[] { "OrganizationId", "EventType", "EventTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Org_ModifiedDate",
                table: "PersonSynToDevice",
                columns: new[] { "OrganizationId", "LastModifiedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonId_DeviceId",
                table: "PersonSynToDevice",
                columns: new[] { "PersonId", "DeviceId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Org_Eventtype_Enroll",
                table: "TimeAttendenceEvent");

            migrationBuilder.DropIndex(
                name: "IX_Org_Eventtype_Eventtime",
                table: "TimeAttendenceEvent");

            migrationBuilder.DropIndex(
                name: "IX_Org_ModifiedDate",
                table: "PersonSynToDevice");

            migrationBuilder.DropIndex(
                name: "IX_PersonId_DeviceId",
                table: "PersonSynToDevice");

            migrationBuilder.CreateIndex(
                name: "IX_TimeAttendenceEvent_CreatedDate",
                table: "TimeAttendenceEvent",
                column: "CreatedDate");
        }
    }
}
