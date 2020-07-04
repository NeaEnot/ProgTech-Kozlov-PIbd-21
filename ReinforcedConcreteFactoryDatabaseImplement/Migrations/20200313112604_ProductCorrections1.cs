using Microsoft.EntityFrameworkCore.Migrations;

namespace ReinforcedConcreteFactoryDatabaseImplement.Migrations
{
    public partial class ProductCorrections1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductComponents_ProductId",
                table: "ProductComponents");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComponents_ProductId",
                table: "ProductComponents",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductComponents_ProductId",
                table: "ProductComponents");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComponents_ProductId",
                table: "ProductComponents",
                column: "ProductId",
                unique: true);
        }
    }
}
