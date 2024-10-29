using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.ZkAutoPush.Datas.Migrations.MySql
{
    public partial class Init_DB1352 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "full_name",
                schema: "Zkteco",
                table: "zk_user",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_name",
                schema: "Zkteco",
                table: "zk_user");
        }
    }
}
