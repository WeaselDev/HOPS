using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOPS.Migrations
{
    /// <inheritdoc />
    public partial class UseBeerStyleInRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Style",
                table: "BrewRecipe");

            migrationBuilder.AddColumn<string>(
                name: "BeerStyleId",
                table: "BrewRecipe",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrewRecipe_BeerStyleId",
                table: "BrewRecipe",
                column: "BeerStyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrewRecipe_BeerStyles_BeerStyleId",
                table: "BrewRecipe",
                column: "BeerStyleId",
                principalTable: "BeerStyles",
                principalColumn: "StyleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrewRecipe_BeerStyles_BeerStyleId",
                table: "BrewRecipe");

            migrationBuilder.DropIndex(
                name: "IX_BrewRecipe_BeerStyleId",
                table: "BrewRecipe");

            migrationBuilder.DropColumn(
                name: "BeerStyleId",
                table: "BrewRecipe");

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "BrewRecipe",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
