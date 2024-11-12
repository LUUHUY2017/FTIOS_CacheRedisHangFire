using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.Hanet.Datas.Migrations.MySql
{
    public partial class Init_DB_Hanet_24111215711444 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_time",
                schema: "Hanet",
                table: "hanet_transaction",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "time",
                schema: "Hanet",
                table: "hanet_transaction",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "transaction_type",
                schema: "Hanet",
                table: "hanet_transaction",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_time",
                schema: "Hanet",
                table: "hanet_transaction");

            migrationBuilder.DropColumn(
                name: "time",
                schema: "Hanet",
                table: "hanet_transaction");

            migrationBuilder.DropColumn(
                name: "transaction_type",
                schema: "Hanet",
                table: "hanet_transaction");
        }
    }
}
