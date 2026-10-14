using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOPS.Migrations
{
    /// <inheritdoc />
    public partial class UseSelectedYeasts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedYeasts",
                table: "BrewBatch",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedYeasts",
                table: "BrewBatch");
        }
    }
}
