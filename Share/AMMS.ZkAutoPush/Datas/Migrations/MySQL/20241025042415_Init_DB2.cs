using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.ZkAutoPush.Datas.Migrations.MySql
{
    public partial class Init_DB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_user",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_transaction",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_terminalcommandlog",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "request_id",
                schema: "Zkteco",
                table: "zk_terminalcommandlog",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "successed",
                schema: "Zkteco",
                table: "zk_terminalcommandlog",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                schema: "Zkteco",
                table: "zk_terminal",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "area_id",
                schema: "Zkteco",
                table: "zk_terminal",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_terminal",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "isconnect",
                schema: "Zkteco",
                table: "zk_terminal",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_biophoto",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_biodata",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "request_id",
                schema: "Zkteco",
                table: "zk_terminalcommandlog");

            migrationBuilder.DropColumn(
                name: "successed",
                schema: "Zkteco",
                table: "zk_terminalcommandlog");

            migrationBuilder.DropColumn(
                name: "isconnect",
                schema: "Zkteco",
                table: "zk_terminal");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_user",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_transaction",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_terminalcommandlog",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                schema: "Zkteco",
                table: "zk_terminal",
                keyColumn: "ip_address",
                keyValue: null,
                column: "ip_address",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ip_address",
                schema: "Zkteco",
                table: "zk_terminal",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                schema: "Zkteco",
                table: "zk_terminal",
                keyColumn: "area_id",
                keyValue: null,
                column: "area_id",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "area_id",
                schema: "Zkteco",
                table: "zk_terminal",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_terminal",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_biophoto",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "Zkteco",
                table: "zk_biodata",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
