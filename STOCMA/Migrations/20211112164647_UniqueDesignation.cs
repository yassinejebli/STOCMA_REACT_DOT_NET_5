using Microsoft.EntityFrameworkCore.Migrations;

namespace STOCMA.Migrations
{
    public partial class UniqueDesignation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Articles_Designation",
                table: "Articles",
                column: "Designation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFactures_Designation",
                table: "ArticleFactures",
                column: "Designation",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Designation",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_ArticleFactures_Designation",
                table: "ArticleFactures");
        }
    }
}
