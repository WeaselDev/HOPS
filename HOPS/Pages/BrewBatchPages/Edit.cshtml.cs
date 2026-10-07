using HOPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Pages.BrewBatchPages;

public class EditModel : PageModel
{
    private readonly HOPSContext _context;

    public EditModel(HOPSContext context)
    {
        _context = context;
    }

    [BindProperty]
    public BrewBatch BrewBatch { get; set; } = default!;

    public BrewRecipe BrewRecipe { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        BrewBatch = await LoadBrewBatchAsync(id.Value);

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

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var brewBatch = await _context.BrewBatch
            .Include(b => b.Hops)
            .Include(b => b.WaterAdditions.OrderBy(a => a.Index))
            .Include(b => b.YeastAdditions.OrderBy(a => a.Index))
            .FirstOrDefaultAsync(b => b.Id == BrewBatch.Id);

        if (brewBatch is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return await ReloadPageAsync(BrewBatch);
        }

        brewBatch.BatchSize = BrewBatch.BatchSize;
        brewBatch.BrewhouseEfficiency = BrewBatch.BrewhouseEfficiency;
        brewBatch.MashWaterPercentage = BrewBatch.MashWaterPercentage;

        brewBatch.Status = BrewBatch.Status;
        brewBatch.BrewNote = BrewBatch.BrewNote;

        brewBatch.BrewDate = BrewBatch.BrewDate;
        brewBatch.FermentationStartDate = BrewBatch.FermentationStartDate;
        brewBatch.FermentationStartPlato = BrewBatch.FermentationStartPlato;
        brewBatch.BottlingDate = BrewBatch.BottlingDate;
        brewBatch.BottlingPlato = BrewBatch.BottlingPlato;
        brewBatch.BottlingTemperature = BrewBatch.BottlingTemperature;
        brewBatch.BottlingVolume = BrewBatch.BottlingVolume;

        brewBatch.ApparentAttenuation = BrewBatch.ApparentAttenuation;
        brewBatch.ActualABV = BrewBatch.ActualABV;

        brewBatch.Rating = BrewBatch.Rating;
        brewBatch.RatingNote = BrewBatch.RatingNote;

        brewBatch.UseIrishMoss = BrewBatch.UseIrishMoss;
        brewBatch.UseWhirlfloc = BrewBatch.UseWhirlfloc;
        brewBatch.UseSilicaSol = BrewBatch.UseSilicaSol;

        foreach (var postedHop in BrewBatch.Hops)
        {
            var existingHop = brewBatch.Hops
                .SingleOrDefault(h =>
                    h.HopAdditionId == postedHop.HopAdditionId);

            if (existingHop is not null)
            {
                existingHop.AlphaAcid = postedHop.AlphaAcid;
            }
        }

        foreach (var postedWater in BrewBatch.WaterAdditions)
        {
            var existingWater = brewBatch.WaterAdditions
                .SingleOrDefault(w => w.Id == postedWater.Id);

            if (existingWater is not null)
            {
                existingWater.Amount = postedWater.Amount;
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToPage(
            "./Details",
            new { id = brewBatch.Id });
    }

    private async Task<BrewBatch?> LoadBrewBatchAsync(Guid id)
    {
        return await _context.BrewBatch
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
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    private async Task<IActionResult> ReloadPageAsync(
        BrewBatch postedBrewBatch)
    {
        BrewBatch = await LoadBrewBatchAsync(postedBrewBatch.Id);

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

        BrewBatch.BatchSize = postedBrewBatch.BatchSize;
        BrewBatch.BrewhouseEfficiency =
            postedBrewBatch.BrewhouseEfficiency;
        BrewBatch.MashWaterPercentage =
            postedBrewBatch.MashWaterPercentage;

        BrewBatch.Status = postedBrewBatch.Status;
        BrewBatch.BrewNote = postedBrewBatch.BrewNote;

        BrewBatch.BrewDate = postedBrewBatch.BrewDate;
        BrewBatch.FermentationStartDate =
            postedBrewBatch.FermentationStartDate;
        BrewBatch.FermentationStartPlato =
            postedBrewBatch.FermentationStartPlato;
        BrewBatch.BottlingDate =
            postedBrewBatch.BottlingDate;
        BrewBatch.BottlingPlato =
            postedBrewBatch.BottlingPlato;

        BrewBatch.ApparentAttenuation =
            postedBrewBatch.ApparentAttenuation;
        BrewBatch.ActualABV =
            postedBrewBatch.ActualABV;

        BrewBatch.Rating = postedBrewBatch.Rating;
        BrewBatch.RatingNote = postedBrewBatch.RatingNote;

        BrewBatch.UseIrishMoss = postedBrewBatch.UseIrishMoss;
        BrewBatch.UseWhirlfloc = postedBrewBatch.UseWhirlfloc;
        BrewBatch.UseSilicaSol = postedBrewBatch.UseSilicaSol;

        foreach (var postedHop in postedBrewBatch.Hops)
        {
            var hop = BrewBatch.Hops
                .SingleOrDefault(h =>
                    h.HopAdditionId == postedHop.HopAdditionId);

            if (hop is not null)
            {
                hop.AlphaAcid = postedHop.AlphaAcid;
            }
        }

        foreach (var postedWater in postedBrewBatch.WaterAdditions)
        {
            var water = BrewBatch.WaterAdditions
                .SingleOrDefault(w => w.Id == postedWater.Id);

            if (water is not null)
            {
                water.Amount = postedWater.Amount;
            }
        }

        return Page();
    }
}