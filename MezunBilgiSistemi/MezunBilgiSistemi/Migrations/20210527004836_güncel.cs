using Microsoft.EntityFrameworkCore.Migrations;

namespace MezunBilgiSistemi.Migrations
{
    public partial class güncel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Duyurulars",
                columns: table => new
                {
                    DuyuruId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuyuruAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuyuruAciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuyuruFotoLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duyurulars", x => x.DuyuruId);
                });

            migrationBuilder.CreateTable(
                name: "Etkinliklers",
                columns: table => new
                {
                    EtkinlikId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EtkinlikAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EtkinlikAciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EtkinlikFotoLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etkinliklers", x => x.EtkinlikId);
                });

            migrationBuilder.CreateTable(
                name: "Haberlers",
                columns: table => new
                {
                    HaberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HaberAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HaberAciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HaberFotoLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Haberlers", x => x.HaberID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Duyurulars");

            migrationBuilder.DropTable(
                name: "Etkinliklers");

            migrationBuilder.DropTable(
                name: "Haberlers");
        }
    }
}
