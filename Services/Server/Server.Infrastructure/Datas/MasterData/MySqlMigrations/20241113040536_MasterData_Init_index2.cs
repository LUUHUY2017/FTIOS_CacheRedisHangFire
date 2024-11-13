using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_index2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TimeAttendenceSync_OrganizationId",
                table: "TimeAttendenceSync",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSynToDevice_LastModifiedDate",
                table: "PersonSynToDevice",
                column: "LastModifiedDate");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSynToDevice_OrganizationId",
                table: "PersonSynToDevice",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeAttendenceSync_OrganizationId",
                table: "TimeAttendenceSync");

            migrationBuilder.DropIndex(
                name: "IX_PersonSynToDevice_LastModifiedDate",
                table: "PersonSynToDevice");

            migrationBuilder.DropIndex(
                name: "IX_PersonSynToDevice_OrganizationId",
                table: "PersonSynToDevice");
        }
    }
}
