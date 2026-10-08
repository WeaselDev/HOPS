using HOPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Pages.BrewBatchPages;

public class PrintLabelModel : PageModel
{
    private readonly HOPSContext _context;

    public PrintLabelModel(HOPSContext context)
    {
        _context = context;
    }

    public LabelModel Label { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var brewBatch = await _context.BrewBatch
            .Include(b => b.BrewRecipeVersion)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (brewBatch == null)
        {
            return NotFound();
        }

        if (brewBatch.IsDeleted)
        {
            return NotFound();
        }

        var recipeVersion = brewBatch.BrewRecipeVersion;

        if (recipeVersion == null)
        {
            return NotFound();
        }

        var brewRecipe = await _context.BrewRecipe
            .FirstOrDefaultAsync(r => r.Id == recipeVersion.BrewRecipeId);

        if (brewRecipe == null)
        {
            return NotFound();
        }

        DateTime? bestToDrinkFrom = null;
        DateTime? bestToDrinkUntil = null;

        if (brewBatch.BottlingDate.HasValue &&
            recipeVersion.MaturationWeeksMin > 0)
        {
            bestToDrinkFrom = brewBatch.BottlingDate.Value
                .AddDays(recipeVersion.MaturationWeeksMin * 7);

            bestToDrinkUntil = brewBatch.BottlingDate.Value
                .AddDays(recipeVersion.MaturationWeeksMax * 7);
        }

        const decimal defaultBottleSize = 0.33m;

        var labelCount = 0;

        if (brewBatch.BottlingVolume.HasValue &&
            brewBatch.BottlingVolume.Value > 0)
        {
            labelCount = (int)Math.Floor(
                brewBatch.BottlingVolume.Value / defaultBottleSize);
        }

        Label = new LabelModel
        {
            BeerName = brewRecipe.Name,
            Style = brewRecipe.Style,
            OriginalPlato = brewBatch.FermentationStartPlato,
            ABV = brewBatch.ActualABV,
            IBU = recipeVersion.Bitterness,
            BottlingDate = brewBatch.BottlingDate,
            BestToDrinkFrom = bestToDrinkFrom,
            BestToDrinkUntil = bestToDrinkUntil,
            BottleSize = defaultBottleSize,
            LabelCount = labelCount
        };

        return Page();
    }

    public class LabelModel
    {
        public string BeerName { get; set; } = string.Empty;

        public string? Style { get; set; }

        public decimal? OriginalPlato { get; set; }

        public decimal? ABV { get; set; }

        public decimal? IBU { get; set; }

        public DateTime? BottlingDate { get; set; }

        public DateTime? BestToDrinkFrom { get; set; }

        public DateTime? BestToDrinkUntil { get; set; }

        public decimal BottleSize { get; set; }

        public int LabelCount { get; set; }
    }
}