using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MezunBilgiSistemi.Migrations
{
    public partial class tarihli : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Haberlers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Etkinliklers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Duyurulars",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Haberlers");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Etkinliklers");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Duyurulars");
        }
    }
}
