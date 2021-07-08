using Microsoft.EntityFrameworkCore.Migrations;

namespace MezunBilgiSistemi.Migrations
{
    public partial class anasayfada1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                table: "Haberlers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                table: "Etkinliklers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                table: "Duyurulars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "Haberlers");

            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "Etkinliklers");

            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "Duyurulars");
        }
    }
}
