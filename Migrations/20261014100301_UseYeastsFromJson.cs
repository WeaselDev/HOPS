using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOPS.Migrations
{
    /// <inheritdoc />
    public partial class UseYeastsFromJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Yeasts",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Producer = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    BestFor = table.Column<string>(type: "TEXT", nullable: true),
                    AttenuationRangeMin = table.Column<decimal>(type: "TEXT", nullable: true),
                    AttenuationRangeMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    Flocculation = table.Column<string>(type: "TEXT", nullable: true),
                    TemperatureRangeMin = table.Column<decimal>(type: "TEXT", nullable: true),
                    TemperatureRangeMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    AlcoholTolerance = table.Column<decimal>(type: "TEXT", nullable: true),
                    Form = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yeasts", x => x.ProductId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Yeasts");
        }
    }
}
