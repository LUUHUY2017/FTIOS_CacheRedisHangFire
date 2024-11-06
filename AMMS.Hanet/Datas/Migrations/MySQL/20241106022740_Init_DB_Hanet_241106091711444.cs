using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.Hanet.Datas.Migrations.MySql
{
    public partial class Init_DB_Hanet_241106091711444 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlateId",
                schema: "Hanet",
                table: "app_config",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlateId",
                schema: "Hanet",
                table: "app_config");
        }
    }
}
