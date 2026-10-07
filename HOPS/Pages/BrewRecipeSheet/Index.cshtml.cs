using HOPS.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Pages.BrewRecipeSheet;

public class BrewRecipeIndexItem
{
    public BrewRecipe BrewRecipe { get; set; } = default!;

    public int BrewCount { get; set; }

    public int RatingCount { get; set; }

    public double? AverageRating { get; set; }
}

public class IndexModel : PageModel
{
    private readonly HOPSContext _context;

    public IndexModel(HOPSContext context)
    {
        _context = context;
    }

    public IList<BrewRecipeIndexItem> Recipes { get; set; } = [];

    public async Task OnGetAsync()
    {
        var recipes = await _context.BrewRecipe
            .Where(r => !r.IsDeleted)
            .Include(r => r.Versions)
            .ToListAsync();

        var versionIds = recipes
            .SelectMany(r => r.Versions)
            .Select(v => v.Id)
            .ToList();

        var brewBatches = await _context.BrewBatch
            .Where(b =>
                !b.IsDeleted &&
                versionIds.Contains(b.BrewRecipeVersionId))
            .ToListAsync();

        Recipes = [.. recipes.Select(recipe =>
        {
            var recipeVersionIds = recipe.Versions
                .Select(v => v.Id)
                .ToHashSet();

            var batches = brewBatches
                .Where(b => recipeVersionIds.Contains(b.BrewRecipeVersionId))
                .ToList();

            var ratings = batches
                .Where(b => b.Rating.HasValue)
                .Select(b => b.Rating!.Value)
                .ToList();

            return new BrewRecipeIndexItem
            {
                BrewRecipe = recipe,
                BrewCount = batches.Count,
                RatingCount = ratings.Count,
                AverageRating = ratings.Count > 0
                    ? ratings.Average()
                    : null
            };
        })];
    }
}