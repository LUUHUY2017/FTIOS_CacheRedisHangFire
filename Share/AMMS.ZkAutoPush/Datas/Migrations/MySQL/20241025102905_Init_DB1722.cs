using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.ZkAutoPush.Datas.Migrations.MySql
{
    public partial class Init_DB1722 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "return_content",
                schema: "Zkteco",
                table: "zk_terminalcommandlog",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "return_content",
                schema: "Zkteco",
                table: "zk_terminalcommandlog");
        }
    }
}
