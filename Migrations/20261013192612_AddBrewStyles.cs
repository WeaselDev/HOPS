using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOPS.Migrations
{
    /// <inheritdoc />
    public partial class AddBrewStyles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeerStyles",
                columns: table => new
                {
                    StyleId = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CategoryDescription = table.Column<string>(type: "TEXT", nullable: true),
                    OverallImpression = table.Column<string>(type: "TEXT", nullable: true),
                    Aroma = table.Column<string>(type: "TEXT", nullable: true),
                    Appearance = table.Column<string>(type: "TEXT", nullable: true),
                    Flavor = table.Column<string>(type: "TEXT", nullable: true),
                    Mouthfeel = table.Column<string>(type: "TEXT", nullable: true),
                    Comments = table.Column<string>(type: "TEXT", nullable: true),
                    History = table.Column<string>(type: "TEXT", nullable: true),
                    StyleComparison = table.Column<string>(type: "TEXT", nullable: true),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    Ingredients = table.Column<string>(type: "TEXT", nullable: true),
                    Examples = table.Column<string>(type: "TEXT", nullable: true),
                    StyleGuide = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    OgMin = table.Column<decimal>(type: "TEXT", nullable: true),
                    OgMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    IbuMin = table.Column<decimal>(type: "TEXT", nullable: true),
                    IbuMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    FgMin = table.Column<decimal>(type: "TEXT", nullable: true),
                    FgMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    AbvMin = table.Column<decimal>(type: "TEXT", nullable: true),
                    AbvMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    ColorMin = table.Column<decimal>(type: "TEXT", nullable: true),
                    ColorMax = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerStyles", x => x.StyleId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeerStyles");
        }
    }
}
