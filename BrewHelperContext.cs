using HOPS.Models;
using Microsoft.EntityFrameworkCore;

public class HOPSContext(DbContextOptions<HOPSContext> options) : DbContext(options)
{
    public DbSet<BrewRecipe> BrewRecipe { get; set; } = default!;
    public DbSet<BrewRecipeVersion> BrewRecipeVersion { get; set; } = default!;
    public DbSet<MaltAddition> MaltAddition { get; set; } = default!;
    public DbSet<HopAddition> HopAddition { get; set; } = default!;
    public DbSet<YeastAddition> YeastAddition { get; set; } = default!;
    public DbSet<MashStep> MashStep { get; set; } = default!;
    public DbSet<FermentationAddition> FermentationAddition { get; set; } = default!;
    public DbSet<BrewBatch> BrewBatch { get; set; } = default!;
    public DbSet<BeerStyle> BeerStyles => Set<BeerStyle>();
}
