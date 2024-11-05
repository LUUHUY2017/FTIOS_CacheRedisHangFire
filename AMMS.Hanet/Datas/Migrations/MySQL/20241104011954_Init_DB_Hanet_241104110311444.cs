using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.Hanet.Datas.Migrations.MySql
{
    public partial class Init_DB_Hanet_241104110311444 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "last_checkconnection",
                schema: "Hanet",
                table: "terminal",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "online_status",
                schema: "Hanet",
                table: "terminal",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "time_offline",
                schema: "Hanet",
                table: "terminal",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "time_online",
                schema: "Hanet",
                table: "terminal",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_checkconnection",
                schema: "Hanet",
                table: "terminal");

            migrationBuilder.DropColumn(
                name: "online_status",
                schema: "Hanet",
                table: "terminal");

            migrationBuilder.DropColumn(
                name: "time_offline",
                schema: "Hanet",
                table: "terminal");

            migrationBuilder.DropColumn(
                name: "time_online",
                schema: "Hanet",
                table: "terminal");
        }
    }
}
