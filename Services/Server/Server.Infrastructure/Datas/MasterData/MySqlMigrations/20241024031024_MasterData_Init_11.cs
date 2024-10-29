using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SynAction",
                table: "A2_PersonSynToDevice",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "SynStatus",
                table: "A2_PersonSynToDevice",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SynStatus",
                table: "A2_PersonSynToDevice");

            migrationBuilder.AlterColumn<sbyte>(
                name: "SynAction",
                table: "A2_PersonSynToDevice",
                type: "tinyint(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
