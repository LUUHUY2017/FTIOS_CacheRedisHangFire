using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Datas.MasterData.MySqlMigrations
{
    public partial class MasterData_Init_error_stuclassyear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassRoom_Organization_OrganizationId",
                table: "ClassRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassRoomYear_Organization_ClassRoomId",
                table: "StudentClassRoomYear");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassRoomYear_Organization_OrganizationId",
                table: "StudentClassRoomYear");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentClassRoomYear_SchoolYear_SchoolYearId",
                table: "StudentClassRoomYear");

            migrationBuilder.DropIndex(
                name: "IX_StudentClassRoomYear_ClassRoomId",
                table: "StudentClassRoomYear");

            migrationBuilder.DropIndex(
                name: "IX_StudentClassRoomYear_OrganizationId",
                table: "StudentClassRoomYear");

            migrationBuilder.DropIndex(
                name: "IX_StudentClassRoomYear_SchoolYearId",
                table: "StudentClassRoomYear");

            migrationBuilder.DropIndex(
                name: "IX_ClassRoom_OrganizationId",
                table: "ClassRoom");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolYearId",
                table: "StudentClassRoomYear",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolId",
                table: "StudentClassRoomYear",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StudentClassRoomYear",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ClassRoomId",
                table: "StudentClassRoomYear",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start",
                table: "SchoolYear",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SchoolYear",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "End",
                table: "SchoolYear",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Depcription",
                table: "SchoolYear",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolId",
                table: "ClassRoom",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClassRoom",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StudentClassRoomYear",
                keyColumn: "SchoolYearId",
                keyValue: null,
                column: "SchoolYearId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolYearId",
                table: "StudentClassRoomYear",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "StudentClassRoomYear",
                keyColumn: "SchoolId",
                keyValue: null,
                column: "SchoolId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolId",
                table: "StudentClassRoomYear",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "StudentClassRoomYear",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StudentClassRoomYear",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "StudentClassRoomYear",
                keyColumn: "ClassRoomId",
                keyValue: null,
                column: "ClassRoomId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ClassRoomId",
                table: "StudentClassRoomYear",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Start",
                table: "SchoolYear",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "SchoolYear",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SchoolYear",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "End",
                table: "SchoolYear",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "SchoolYear",
                keyColumn: "Depcription",
                keyValue: null,
                column: "Depcription",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Depcription",
                table: "SchoolYear",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ClassRoom",
                keyColumn: "SchoolId",
                keyValue: null,
                column: "SchoolId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolId",
                table: "ClassRoom",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ClassRoom",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClassRoom",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassRoomYear_ClassRoomId",
                table: "StudentClassRoomYear",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassRoomYear_OrganizationId",
                table: "StudentClassRoomYear",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassRoomYear_SchoolYearId",
                table: "StudentClassRoomYear",
                column: "SchoolYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRoom_OrganizationId",
                table: "ClassRoom",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRoom_Organization_OrganizationId",
                table: "ClassRoom",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassRoomYear_Organization_ClassRoomId",
                table: "StudentClassRoomYear",
                column: "ClassRoomId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassRoomYear_Organization_OrganizationId",
                table: "StudentClassRoomYear",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentClassRoomYear_SchoolYear_SchoolYearId",
                table: "StudentClassRoomYear",
                column: "SchoolYearId",
                principalTable: "SchoolYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
