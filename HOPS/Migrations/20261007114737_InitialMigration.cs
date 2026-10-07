using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOPS.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrewRecipe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Style = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewRecipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrewRecipeVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    BrewRecipeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    BatchSize = table.Column<int>(type: "INTEGER", nullable: false),
                    BrewhouseEfficiency = table.Column<decimal>(type: "TEXT", nullable: false),
                    OriginalGravity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Bitterness = table.Column<int>(type: "INTEGER", nullable: false),
                    Color = table.Column<decimal>(type: "TEXT", nullable: false),
                    EstimatedABV = table.Column<decimal>(type: "TEXT", nullable: false),
                    ApparantAttenuation = table.Column<decimal>(type: "TEXT", nullable: false),
                    FermentationTemperature = table.Column<int>(type: "INTEGER", nullable: false),
                    Carbonation = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalWater = table.Column<decimal>(type: "TEXT", nullable: false),
                    MashWaterPercentage = table.Column<decimal>(type: "TEXT", nullable: false),
                    BoilDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    UseIrishMoss = table.Column<bool>(type: "INTEGER", nullable: false),
                    UseWhirlfloc = table.Column<bool>(type: "INTEGER", nullable: false),
                    UseSilicaSol = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaturationWeeksMin = table.Column<int>(type: "INTEGER", nullable: false),
                    MaturationWeeksMax = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewRecipeVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrewRecipeVersion_BrewRecipe_BrewRecipeId",
                        column: x => x.BrewRecipeId,
                        principalTable: "BrewRecipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrewBatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BrewRecipeVersionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    BatchSize = table.Column<int>(type: "INTEGER", nullable: false),
                    BrewhouseEfficiency = table.Column<decimal>(type: "TEXT", nullable: false),
                    MashWaterPercentage = table.Column<decimal>(type: "TEXT", nullable: false),
                    UseIrishMoss = table.Column<bool>(type: "INTEGER", nullable: false),
                    UseWhirlfloc = table.Column<bool>(type: "INTEGER", nullable: false),
                    UseSilicaSol = table.Column<bool>(type: "INTEGER", nullable: false),
                    BrewNote = table.Column<string>(type: "TEXT", nullable: true),
                    BrewDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FermentationStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FermentationStartPlato = table.Column<decimal>(type: "TEXT", nullable: true),
                    BottlingDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BottlingPlato = table.Column<decimal>(type: "TEXT", nullable: true),
                    BottlingTemperature = table.Column<decimal>(type: "TEXT", nullable: true),
                    BottlingVolume = table.Column<decimal>(type: "TEXT", nullable: true),
                    ApparentAttenuation = table.Column<decimal>(type: "TEXT", nullable: true),
                    ActualABV = table.Column<decimal>(type: "TEXT", nullable: true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true),
                    RatingNote = table.Column<string>(type: "TEXT", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrewBatch_BrewRecipeVersion_BrewRecipeVersionId",
                        column: x => x.BrewRecipeVersionId,
                        principalTable: "BrewRecipeVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FermentationAddition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Time = table.Column<string>(type: "TEXT", nullable: false),
                    BrewRecipeVersionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FermentationAddition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FermentationAddition_BrewRecipeVersion_BrewRecipeVersionId",
                        column: x => x.BrewRecipeVersionId,
                        principalTable: "BrewRecipeVersion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HopAddition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlphaAcid = table.Column<decimal>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    BrewRecipeVersionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopAddition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HopAddition_BrewRecipeVersion_BrewRecipeVersionId",
                        column: x => x.BrewRecipeVersionId,
                        principalTable: "BrewRecipeVersion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaltAddition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BrewRecipeVersionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaltAddition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaltAddition_BrewRecipeVersion_BrewRecipeVersionId",
                        column: x => x.BrewRecipeVersionId,
                        principalTable: "BrewRecipeVersion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MashStep",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Temperature = table.Column<double>(type: "REAL", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    BrewRecipeVersionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MashStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MashStep_BrewRecipeVersion_BrewRecipeVersionId",
                        column: x => x.BrewRecipeVersionId,
                        principalTable: "BrewRecipeVersion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BrewHop",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    HopAdditionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlphaAcid = table.Column<decimal>(type: "TEXT", nullable: false),
                    BrewBatchId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewHop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrewHop_BrewBatch_BrewBatchId",
                        column: x => x.BrewBatchId,
                        principalTable: "BrewBatch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WaterAddition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    BrewBatchId = table.Column<Guid>(type: "TEXT", nullable: true),
                    BrewRecipeVersionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterAddition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterAddition_BrewBatch_BrewBatchId",
                        column: x => x.BrewBatchId,
                        principalTable: "BrewBatch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WaterAddition_BrewRecipeVersion_BrewRecipeVersionId",
                        column: x => x.BrewRecipeVersionId,
                        principalTable: "BrewRecipeVersion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YeastAddition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    BrewBatchId = table.Column<Guid>(type: "TEXT", nullable: true),
                    BrewRecipeVersionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeastAddition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YeastAddition_BrewBatch_BrewBatchId",
                        column: x => x.BrewBatchId,
                        principalTable: "BrewBatch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_YeastAddition_BrewRecipeVersion_BrewRecipeVersionId",
                        column: x => x.BrewRecipeVersionId,
                        principalTable: "BrewRecipeVersion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrewBatch_BrewRecipeVersionId",
                table: "BrewBatch",
                column: "BrewRecipeVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_BrewHop_BrewBatchId",
                table: "BrewHop",
                column: "BrewBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BrewRecipeVersion_BrewRecipeId",
                table: "BrewRecipeVersion",
                column: "BrewRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_FermentationAddition_BrewRecipeVersionId",
                table: "FermentationAddition",
                column: "BrewRecipeVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_HopAddition_BrewRecipeVersionId",
                table: "HopAddition",
                column: "BrewRecipeVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaltAddition_BrewRecipeVersionId",
                table: "MaltAddition",
                column: "BrewRecipeVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_MashStep_BrewRecipeVersionId",
                table: "MashStep",
                column: "BrewRecipeVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterAddition_BrewBatchId",
                table: "WaterAddition",
                column: "BrewBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterAddition_BrewRecipeVersionId",
                table: "WaterAddition",
                column: "BrewRecipeVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_YeastAddition_BrewBatchId",
                table: "YeastAddition",
                column: "BrewBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_YeastAddition_BrewRecipeVersionId",
                table: "YeastAddition",
                column: "BrewRecipeVersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrewHop");

            migrationBuilder.DropTable(
                name: "FermentationAddition");

            migrationBuilder.DropTable(
                name: "HopAddition");

            migrationBuilder.DropTable(
                name: "MaltAddition");

            migrationBuilder.DropTable(
                name: "MashStep");

            migrationBuilder.DropTable(
                name: "WaterAddition");

            migrationBuilder.DropTable(
                name: "YeastAddition");

            migrationBuilder.DropTable(
                name: "BrewBatch");

            migrationBuilder.DropTable(
                name: "BrewRecipeVersion");

            migrationBuilder.DropTable(
                name: "BrewRecipe");
        }
    }
}
