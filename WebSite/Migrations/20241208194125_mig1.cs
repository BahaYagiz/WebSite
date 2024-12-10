using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebSite.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Türkiye" },
                    { 2, true, "Gündem" },
                    { 3, true, "Dünya" },
                    { 4, true, "Spor" },
                    { 5, true, "Ekonomi" },
                    { 6, true, "Sağlık" },
                    { 7, true, "Politika" }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "AuthorName", "CategoryId", "Content", "Description", "IsActive", "PhotoUrl", "Title" },
                values: new object[,]
                {
                    { 1, "Mehmet Yılmaz", 1, "Detaylı içerik...", "Türkiye'deki son gelişmeler ve haberler.", true, "/uploads/turkiye.jpg", "Türkiye'deki Son Durum" },
                    { 2, "Ayşe Demir", 2, "Gündem ile ilgili tüm detaylar...", "Ülkedeki gündem maddeleri hakkında detaylar.", true, "/uploads/gundem.jpg", "Gündemdeki En Önemli Konular" },
                    { 3, "Ali Veli", 3, "Dünya ekonomisindeki son değişiklikler...", "Dünya ekonomisi üzerindeki etkileyen faktörler.", true, "/uploads/dunya.jpg", "Dünya Ekonomisinde Son Gelişmeler" },
                    { 4, "Fatma Kaya", 4, "Sporun geleceği hakkında detaylı bir analiz...", "Yeni sporcuların başarıları.", true, "/uploads/spor.jpg", "Sporun Yükselen Yıldızları" },
                    { 5, "Ahmet Öztürk", 5, "Ekonomik kriz ile ilgili çeşitli analizler...", "Ekonomik kriz ve dünya üzerindeki etkileri.", true, "/uploads/ekonomi.jpg", "Ekonomik Kriz ve Etkileri" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CategoryId",
                table: "Reports",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
