using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMS.Hanet.Datas.Migrations.MySQL
{
    public partial class Init_DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Hanet");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "terminal",
                schema: "Hanet",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    area_id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    change_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    create_user = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    change_user = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sn = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ip_address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    port = table.Column<int>(type: "int", nullable: true),
                    user_count = table.Column<int>(type: "int", nullable: true),
                    face_count = table.Column<int>(type: "int", nullable: true),
                    fv_count = table.Column<int>(type: "int", nullable: true),
                    last_activity = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    upload_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    push_time = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_terminal", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "terminal",
                schema: "Hanet");
        }
    }
}
