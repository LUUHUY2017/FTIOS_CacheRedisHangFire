using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.Hanet.Datas.Migrations.MySql
{
    public partial class Init_DB_Hanet_24111111711444 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_transaction",
                schema: "Hanet",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_terminal",
                schema: "Hanet",
                table: "terminal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_commandlog",
                schema: "Hanet",
                table: "commandlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_app_config",
                schema: "Hanet",
                table: "app_config");

            migrationBuilder.RenameTable(
                name: "transaction",
                schema: "Hanet",
                newName: "hanet_transaction",
                newSchema: "Hanet");

            migrationBuilder.RenameTable(
                name: "terminal",
                schema: "Hanet",
                newName: "hanet_terminal",
                newSchema: "Hanet");

            migrationBuilder.RenameTable(
                name: "commandlog",
                schema: "Hanet",
                newName: "hanet_commandlog",
                newSchema: "Hanet");

            migrationBuilder.RenameTable(
                name: "app_config",
                schema: "Hanet",
                newName: "hanet_app_config",
                newSchema: "Hanet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hanet_transaction",
                schema: "Hanet",
                table: "hanet_transaction",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hanet_terminal",
                schema: "Hanet",
                table: "hanet_terminal",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hanet_commandlog",
                schema: "Hanet",
                table: "hanet_commandlog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hanet_app_config",
                schema: "Hanet",
                table: "hanet_app_config",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_hanet_transaction",
                schema: "Hanet",
                table: "hanet_transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hanet_terminal",
                schema: "Hanet",
                table: "hanet_terminal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hanet_commandlog",
                schema: "Hanet",
                table: "hanet_commandlog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hanet_app_config",
                schema: "Hanet",
                table: "hanet_app_config");

            migrationBuilder.RenameTable(
                name: "hanet_transaction",
                schema: "Hanet",
                newName: "transaction",
                newSchema: "Hanet");

            migrationBuilder.RenameTable(
                name: "hanet_terminal",
                schema: "Hanet",
                newName: "terminal",
                newSchema: "Hanet");

            migrationBuilder.RenameTable(
                name: "hanet_commandlog",
                schema: "Hanet",
                newName: "commandlog",
                newSchema: "Hanet");

            migrationBuilder.RenameTable(
                name: "hanet_app_config",
                schema: "Hanet",
                newName: "app_config",
                newSchema: "Hanet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transaction",
                schema: "Hanet",
                table: "transaction",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_terminal",
                schema: "Hanet",
                table: "terminal",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_commandlog",
                schema: "Hanet",
                table: "commandlog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_app_config",
                schema: "Hanet",
                table: "app_config",
                column: "Id");
        }
    }
}
