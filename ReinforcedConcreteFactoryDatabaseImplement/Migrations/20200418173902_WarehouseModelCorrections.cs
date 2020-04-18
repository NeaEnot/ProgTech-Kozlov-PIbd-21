using Microsoft.EntityFrameworkCore.Migrations;

namespace ReinforcedConcreteFactoryDatabaseImplement.Migrations
{
    public partial class WarehouseModelCorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseComponents_WarehouseId",
                table: "WarehouseComponents");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseComponents_WarehouseId",
                table: "WarehouseComponents",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseComponents_WarehouseId",
                table: "WarehouseComponents");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseComponents_WarehouseId",
                table: "WarehouseComponents",
                column: "WarehouseId",
                unique: true);
        }
    }
}
