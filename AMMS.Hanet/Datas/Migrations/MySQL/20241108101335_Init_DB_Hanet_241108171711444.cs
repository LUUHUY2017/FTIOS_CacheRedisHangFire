using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.Hanet.Datas.Migrations.MySql
{
    public partial class Init_DB_Hanet_241108171711444 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlateId",
                schema: "Hanet",
                table: "app_config",
                newName: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlaceId",
                schema: "Hanet",
                table: "app_config",
                newName: "PlateId");
        }
    }
}
