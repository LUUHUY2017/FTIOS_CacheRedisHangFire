using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class AddField1ForAttConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndpointGateway",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EndpointIdentity",
                table: "A0_AttendanceConfig",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndpointGateway",
                table: "A0_AttendanceConfig");

            migrationBuilder.DropColumn(
                name: "EndpointIdentity",
                table: "A0_AttendanceConfig");
        }
    }
}
