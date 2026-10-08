using HOPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Pages.BrewBatchPages;

public class CreateModel : PageModel
{
    private readonly HOPSContext _context;

    public CreateModel(HOPSContext context)
    {
        _context = context;
    }

    [BindProperty]
    public BrewBatch BrewBatch { get; set; } = default!;

    public BrewRecipeVersion BrewRecipeVersion { get; set; } = null!;

    public BrewRecipe BrewRecipe { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(Guid recipeVersionId)
    {
        BrewRecipeVersion = await _context.BrewRecipeVersion
            .Include(v => v.MashSteps.OrderBy(m => m.Index))
            .Include(v => v.MaltAdditions.OrderBy(m => m.Index))
            .Include(v => v.YeastAdditions.OrderBy(m => m.Index))
            .Include(v => v.HopAdditions.OrderBy(m => m.Index))
            .Include(v => v.WaterAdditions.OrderBy(m => m.Index))
            .Include(v => v.FermentationAdditions.OrderBy(m => m.Index))
            .FirstOrDefaultAsync(v => v.Id == recipeVersionId);

        if (BrewRecipeVersion is null)
        {
            return NotFound();
        }

        BrewRecipe = await _context.BrewRecipe
            .FirstOrDefaultAsync(r => r.Id == BrewRecipeVersion.BrewRecipeId);

        if (BrewRecipe is null)
        {
            return NotFound();
        }

        BrewBatch = new BrewBatch
        {
            BrewRecipeVersionId = BrewRecipeVersion.Id,

            // später aus Config:
            BatchSize = BrewRecipeVersion.BatchSize,
            BrewhouseEfficiency = BrewRecipeVersion.BrewhouseEfficiency,
            MashWaterPercentage = BrewRecipeVersion.MashWaterPercentage,

            BottlingVolume = BrewRecipeVersion.BatchSize,
            BottlingTemperature = BrewRecipeVersion.FermentationTemperature,

            UseIrishMoss = BrewRecipeVersion.UseIrishMoss,
            UseWhirlfloc = BrewRecipeVersion.UseWhirlfloc,
            UseSilicaSol = BrewRecipeVersion.UseSilicaSol,

            Hops =
            [
                .. BrewRecipeVersion.HopAdditions.Select(h => new BrewHop
                {
                    HopAdditionId = h.Id,
                    AlphaAcid = h.AlphaAcid
                })
            ],

            YeastAdditions =
            [
                .. BrewRecipeVersion.YeastAdditions.Select(a => a.CreateCopy())
            ],

            WaterAdditions =
            [
                .. BrewRecipeVersion.WaterAdditions.Select(a => a.CreateCopy())
            ]
        };

        foreach (var yeast in BrewBatch.YeastAdditions)
        {
            var recipeYeast = BrewRecipeVersion.YeastAdditions
                .Single(y => y.Index == yeast.Index);

            yeast.Amount = recipeYeast.CalculateAmount(
                BrewRecipeVersion.BatchSize,
                BrewBatch.BatchSize);
        }
        if (BrewBatch.YeastAdditions.Count > 0)
        {
            BrewBatch.YeastAdditions[0].Selected = true;
        }

        foreach (var water in BrewBatch.WaterAdditions)
        {
            var recipeWater = BrewRecipeVersion.WaterAdditions
                .Single(w => w.Index == water.Index);

            water.Amount = recipeWater.CalculateAmount(
                BrewRecipeVersion.BatchSize,
                BrewBatch.BatchSize);

            water.Name = water.Type.ToString();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        BrewBatch.YeastAdditions = BrewBatch.YeastAdditions
            .Where(y => y.Selected)
            .ToList();
        BrewBatch.BottlingTemperature = BrewRecipeVersion.FermentationTemperature;


        _context.BrewBatch.Add(BrewBatch);

        await _context.SaveChangesAsync();

        return RedirectToPage(
            "./Details",
            new { id = BrewBatch.Id });
    }
}