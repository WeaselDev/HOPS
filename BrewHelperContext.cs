using Microsoft.EntityFrameworkCore;

public class HOPSContext(DbContextOptions<HOPSContext> options) : DbContext(options)
{
    public DbSet<HOPS.Models.BrewRecipe> BrewRecipe { get; set; } = default!;
    public DbSet<HOPS.Models.BrewRecipeVersion> BrewRecipeVersion { get; set; } = default!;
    public DbSet<HOPS.Models.MaltAddition> MaltAddition { get; set; } = default!;
    public DbSet<HOPS.Models.HopAddition> HopAddition { get; set; } = default!;
    public DbSet<HOPS.Models.YeastAddition> YeastAddition { get; set; } = default!;
    public DbSet<HOPS.Models.MashStep> MashStep { get; set; } = default!;
    public DbSet<HOPS.Models.FermentationAddition> FermentationAddition { get; set; } = default!;
    public DbSet<HOPS.Models.BrewBatch> BrewBatch { get; set; } = default!;
}
