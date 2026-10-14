using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOPS.Migrations
{
    /// <inheritdoc />
    public partial class UsePreferredYeasts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YeastAddition_BrewRecipeVersion_BrewRecipeVersionId",
                table: "YeastAddition");

            migrationBuilder.DropIndex(
                name: "IX_YeastAddition_BrewRecipeVersionId",
                table: "YeastAddition");

            migrationBuilder.DropColumn(
                name: "BrewRecipeVersionId",
                table: "YeastAddition");

            migrationBuilder.AddColumn<Guid>(
                name: "BrewRecipeVersionId",
                table: "Yeasts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Yeasts_BrewRecipeVersionId",
                table: "Yeasts",
                column: "BrewRecipeVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Yeasts_BrewRecipeVersion_BrewRecipeVersionId",
                table: "Yeasts",
                column: "BrewRecipeVersionId",
                principalTable: "BrewRecipeVersion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yeasts_BrewRecipeVersion_BrewRecipeVersionId",
                table: "Yeasts");

            migrationBuilder.DropIndex(
                name: "IX_Yeasts_BrewRecipeVersionId",
                table: "Yeasts");

            migrationBuilder.DropColumn(
                name: "BrewRecipeVersionId",
                table: "Yeasts");

            migrationBuilder.AddColumn<Guid>(
                name: "BrewRecipeVersionId",
                table: "YeastAddition",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YeastAddition_BrewRecipeVersionId",
                table: "YeastAddition",
                column: "BrewRecipeVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_YeastAddition_BrewRecipeVersion_BrewRecipeVersionId",
                table: "YeastAddition",
                column: "BrewRecipeVersionId",
                principalTable: "BrewRecipeVersion",
                principalColumn: "Id");
        }
    }
}
