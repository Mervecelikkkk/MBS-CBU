using Microsoft.EntityFrameworkCore.Migrations;

namespace MezunBilgiSistemi.Migrations.MBSDb
{
    public partial class anasayfada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "AspNetUsers");
        }
    }
}
