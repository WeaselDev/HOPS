using HOPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Pages.BrewBatchPages;

public class DetailsModel : PageModel
{
    private readonly HOPSContext _context;

    public DetailsModel(HOPSContext context)
    {
        _context = context;
    }

    public BrewBatch BrewBatch { get; set; } = default!;

    public BrewRecipe BrewRecipe { get; set; } = default!;

    public List<IngredientHop> IngredientHops { get; set; } = [];

    public List<BrewAddition> BrewAdditions { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        BrewBatch = await _context.BrewBatch
            .Include(b => b.BrewRecipeVersion)
                .ThenInclude(v => v.MaltAdditions.OrderBy(a => a.Index))
            .Include(b => b.BrewRecipeVersion)
                .ThenInclude(v => v.HopAdditions.OrderBy(a => a.Index))
            .Include(b => b.BrewRecipeVersion)
                .ThenInclude(v => v.YeastAdditions.OrderBy(a => a.Index))
            .Include(b => b.BrewRecipeVersion)
                .ThenInclude(v => v.WaterAdditions.OrderBy(a => a.Index))
            .Include(b => b.BrewRecipeVersion)
                .ThenInclude(v => v.FermentationAdditions.OrderBy(a => a.Index))
            .Include(b => b.BrewRecipeVersion)
                .ThenInclude(v => v.MashSteps.OrderBy(a => a.Index))
            .Include(b => b.Hops)
            .Include(b => b.WaterAdditions.OrderBy(a => a.Index))
            .Include(b => b.YeastAdditions.OrderBy(a => a.Index))
            .FirstOrDefaultAsync(b =>
                b.Id == id &&
                !b.IsDeleted);

        if (BrewBatch?.BrewRecipeVersion is null)
        {
            return NotFound();
        }

        BrewRecipe = await _context.BrewRecipe
            .FirstOrDefaultAsync(r =>
                r.Id == BrewBatch.BrewRecipeVersion.BrewRecipeId);

        if (BrewRecipe is null)
        {
            return NotFound();
        }

        PrepareDisplayData();

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var brewBatch = await _context.BrewBatch
            .FirstOrDefaultAsync(b =>
                b.Id == id &&
                !b.IsDeleted);

        if (brewBatch is null)
        {
            return NotFound();
        }

        brewBatch.IsDeleted = true;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private void PrepareDisplayData()
    {
        var recipeVersion = BrewBatch.BrewRecipeVersion!;

        IngredientHops = [];

        BrewAdditions = [];

        foreach (var brewHop in BrewBatch.Hops)
        {
            var recipeHop = recipeVersion.HopAdditions
                .Single(h => h.Id == brewHop.HopAdditionId);

            var amount = recipeHop.CalculateAmount(
                recipeVersion.BatchSize,
                BrewBatch.BatchSize,
                recipeHop.AlphaAcid,
                brewHop.AlphaAcid);

            BrewAdditions.Add(new BrewAddition
            {
                Name = recipeHop.Name,
                AlphaAcid = brewHop.AlphaAcid,
                Amount = amount,
                Unit = recipeHop.Unit,
                Duration = recipeHop.Duration,
                TimerTime = recipeVersion.BoilDuration - recipeHop.Duration
            });
        }

        if (BrewBatch.UseIrishMoss)
        {
            var addition = new IrishMossAddition
            {
                TimerTime = recipeVersion.BoilDuration - 15
            };

            BrewAdditions.Add(addition);
        }

        if (BrewBatch.UseWhirlfloc)
        {
            var addition = new WhirlflocAddition
            {
                TimerTime = recipeVersion.BoilDuration - 15
            };

            BrewAdditions.Add(addition);
        }

        BrewAdditions =
        [
            .. BrewAdditions
                .OrderByDescending(a => a.Duration)
        ];

        IngredientHops =
        [
            .. BrewAdditions
                .Where(a => !a.IsVirtual)
                .GroupBy(a => new
                {
                    a.Name,
                    a.AlphaAcid,
                    a.Unit
                })
                .Select(g => new IngredientHop
                {
                    Name = g.Key.Name,
                    AlphaAcid = g.Key.AlphaAcid,
                    Amount = g.Sum(a => a.Amount),
                    Unit = g.Key.Unit
                })
        ];
    }
}