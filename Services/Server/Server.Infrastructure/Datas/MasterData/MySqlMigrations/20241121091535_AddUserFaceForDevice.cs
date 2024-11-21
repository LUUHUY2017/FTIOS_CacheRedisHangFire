using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class AddUserFaceForDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FaceCount",
                table: "Device",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserCount",
                table: "Device",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceCount",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "UserCount",
                table: "Device");
        }
    }
}
