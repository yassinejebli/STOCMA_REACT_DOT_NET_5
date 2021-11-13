using Microsoft.EntityFrameworkCore.Migrations;

namespace STOCMA.Migrations
{
    public partial class UniqueColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sites_Name",
                table: "Sites",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revendeurs_Name",
                table: "Revendeurs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fournisseurs_Name",
                table: "Fournisseurs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Familles_Name",
                table: "Familles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name",
                table: "Clients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sites_Name",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Revendeurs_Name",
                table: "Revendeurs");

            migrationBuilder.DropIndex(
                name: "IX_Fournisseurs_Name",
                table: "Fournisseurs");

            migrationBuilder.DropIndex(
                name: "IX_Familles_Name",
                table: "Familles");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Name",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");
        }
    }
}
