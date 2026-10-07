using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOPS.Models;

namespace HOPS.Pages.BrewRecipePages;

public class EditModel : PageModel
{
    private readonly HOPSContext _context;

    public EditModel(HOPSContext context)
    {
        _context = context;
    }

    [BindProperty]
    public BrewRecipe BrewRecipe { get; set; } = default!;

    [BindProperty]
    public BrewRecipeVersion BrewRecipeVersion { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(System.Guid? id, int? version)
    {
        if (id is null)
        {
            return NotFound();
        }

        var brewrecipe = await _context.BrewRecipe
            .Include(r => r.Versions)
            .ThenInclude(v => v.MashSteps.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.HopAdditions.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.MaltAdditions.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.YeastAdditions.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.FermentationAdditions.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.WaterAdditions.OrderBy(x => x.Index))
            .FirstOrDefaultAsync(r => r.Id == id);

        if (brewrecipe is null)
        {
            return NotFound();
        }

        BrewRecipeVersion = version.HasValue
            ? brewrecipe.Versions.First(v => v.Version == version.Value)
            : brewrecipe.Versions.OrderByDescending(v => v.Version).First();

        BrewRecipe = brewrecipe;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var brewRecipe = await _context.BrewRecipe
            .Include(r => r.Versions)
                .ThenInclude(v => v.MashSteps)
            .Include(r => r.Versions)
                .ThenInclude(v => v.HopAdditions)
            .Include(r => r.Versions)
                .ThenInclude(v => v.MaltAdditions)
            .Include(r => r.Versions)
                .ThenInclude(v => v.YeastAdditions)
            .Include(r => r.Versions)
                .ThenInclude(v => v.FermentationAdditions)
            .Include(r => r.Versions)
                .ThenInclude(v => v.WaterAdditions)
            .FirstOrDefaultAsync(r => r.Id == BrewRecipe.Id);

        if (brewRecipe == null)
        {
            return NotFound();
        }

        var recipeVersion = brewRecipe.Versions
            .FirstOrDefault(v => v.Id == BrewRecipeVersion.Id);

        if (recipeVersion == null)
        {
            return NotFound();
        }

        // Allgemeine Rezeptdaten dürfen unabhängig von der Version geändert werden.
        brewRecipe.Name = BrewRecipe.Name;
        brewRecipe.Style = BrewRecipe.Style;
        brewRecipe.Description = BrewRecipe.Description;

        bool versionWasBrewed = await _context.BrewBatch
            .AnyAsync(b => b.BrewRecipeVersionId == recipeVersion.Id);

        if (versionWasBrewed)
        {
            CreateNewVersion(brewRecipe);
        }
        else
        {
            UpdateVersion(recipeVersion);
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private void UpdateVersion(BrewRecipeVersion recipeVersion)
    {
        recipeVersion.Notes = BrewRecipeVersion.Notes;
        recipeVersion.BatchSize = BrewRecipeVersion.BatchSize;
        recipeVersion.BrewhouseEfficiency = BrewRecipeVersion.BrewhouseEfficiency;
        recipeVersion.OriginalGravity = BrewRecipeVersion.OriginalGravity;
        recipeVersion.Bitterness = BrewRecipeVersion.Bitterness;
        recipeVersion.Color = BrewRecipeVersion.Color;
        recipeVersion.EstimatedABV = BrewRecipeVersion.EstimatedABV;
        recipeVersion.ApparantAttenuation = BrewRecipeVersion.ApparantAttenuation;
        recipeVersion.FermentationTemperature = BrewRecipeVersion.FermentationTemperature;
        recipeVersion.Carbonation = BrewRecipeVersion.Carbonation;

        recipeVersion.TotalWater = BrewRecipeVersion.TotalWater;
        recipeVersion.MashWaterPercentage = BrewRecipeVersion.MashWaterPercentage;

        recipeVersion.BoilDuration = BrewRecipeVersion.BoilDuration;

        recipeVersion.UseIrishMoss = BrewRecipeVersion.UseIrishMoss;
        recipeVersion.UseWhirlfloc = BrewRecipeVersion.UseWhirlfloc;
        recipeVersion.UseSilicaSol = BrewRecipeVersion.UseSilicaSol;

        SyncCollection(
            recipeVersion.MashSteps,
            BrewRecipeVersion.MashSteps,
            x => x.Id,
            (existing, posted) =>
            {
                existing.Index = posted.Index;
                existing.Name = posted.Name;
                existing.Temperature = posted.Temperature;
                existing.Duration = posted.Duration;
            });

        SyncCollection(
            recipeVersion.HopAdditions,
            BrewRecipeVersion.HopAdditions,
            x => x.Id,
            (existing, posted) =>
            {
                existing.Index = posted.Index;
                existing.Name = posted.Name;
                existing.AlphaAcid = posted.AlphaAcid;
                existing.Amount = posted.Amount;
                existing.Unit = posted.Unit;
                existing.Duration = posted.Duration;
            });

        SyncCollection(
            recipeVersion.YeastAdditions,
            BrewRecipeVersion.YeastAdditions,
            x => x.Id,
            (existing, posted) =>
            {
                existing.Index = posted.Index;
                existing.Name = posted.Name;
                existing.Amount = posted.Amount;
                existing.Unit = posted.Unit;
                existing.Description = posted.Description;
            });

        SyncCollection(
            recipeVersion.MaltAdditions,
            BrewRecipeVersion.MaltAdditions,
            x => x.Id,
            (existing, posted) =>
            {
                existing.Index = posted.Index;
                existing.Name = posted.Name;
                existing.Amount = posted.Amount;
                existing.Unit = posted.Unit;
            });

        SyncCollection(
            recipeVersion.FermentationAdditions,
            BrewRecipeVersion.FermentationAdditions,
            x => x.Id,
            (existing, posted) =>
            {
                existing.Index = posted.Index;
                existing.Name = posted.Name;
                existing.Amount = posted.Amount;
                existing.Unit = posted.Unit;
                existing.Time = posted.Time;
            });

        SyncCollection(
            recipeVersion.WaterAdditions,
            BrewRecipeVersion.WaterAdditions,
            x => x.Id,
            (existing, posted) =>
            {
                existing.Index = posted.Index;
                existing.Type = posted.Type;
                existing.Amount = posted.Amount;
                existing.Unit = posted.Unit;
            });
    }

    private void CreateNewVersion(BrewRecipe brewRecipe)
    {
        var newVersion = new BrewRecipeVersion
        {
            Version = brewRecipe.Versions.Max(v => v.Version) + 1,
            BrewRecipeId = brewRecipe.Id,

            Notes = BrewRecipeVersion.Notes,
            BatchSize = BrewRecipeVersion.BatchSize,
            BrewhouseEfficiency = BrewRecipeVersion.BrewhouseEfficiency,
            OriginalGravity = BrewRecipeVersion.OriginalGravity,
            Bitterness = BrewRecipeVersion.Bitterness,
            Color = BrewRecipeVersion.Color,
            EstimatedABV = BrewRecipeVersion.EstimatedABV,
            ApparantAttenuation = BrewRecipeVersion.ApparantAttenuation,
            FermentationTemperature = BrewRecipeVersion.FermentationTemperature,
            Carbonation = BrewRecipeVersion.Carbonation,

            TotalWater = BrewRecipeVersion.TotalWater,
            MashWaterPercentage = BrewRecipeVersion.MashWaterPercentage,

            BoilDuration = BrewRecipeVersion.BoilDuration,

            UseIrishMoss = BrewRecipeVersion.UseIrishMoss,
            UseWhirlfloc = BrewRecipeVersion.UseWhirlfloc,
            UseSilicaSol = BrewRecipeVersion.UseSilicaSol,

            MaltAdditions = [.. BrewRecipeVersion.MaltAdditions.Select(a => a.CreateCopy())],
            HopAdditions = [.. BrewRecipeVersion.HopAdditions.Select(a => a.CreateCopy())],
            YeastAdditions = [.. BrewRecipeVersion.YeastAdditions.Select(a => a.CreateCopy())],
            MashSteps = [.. BrewRecipeVersion.MashSteps.Select(a => a.CreateCopy())],
            FermentationAdditions = [.. BrewRecipeVersion.FermentationAdditions.Select(a => a.CreateCopy())],
            WaterAdditions = [.. BrewRecipeVersion.WaterAdditions.Select(a => a.CreateCopy())]
        };

        brewRecipe.Versions.Add(newVersion);
    }

    private void SyncCollection<T>(
    ICollection<T> existingItems,
    ICollection<T> postedItems,
    Func<T, Guid> getId,
    Action<T, T> update)
    where T : class
    {
        // Gelöschte entfernen
        var removedItems = existingItems
            .Where(existing =>
                !postedItems.Any(posted => getId(posted) == getId(existing)))
            .ToList();

        _context.RemoveRange(removedItems);

        // Bestehende aktualisieren
        foreach (var posted in postedItems.Where(x => getId(x) != Guid.Empty))
        {
            var existing = existingItems
                .First(x => getId(x) == getId(posted));

            update(existing, posted);
        }

        // Neue hinzufügen
        foreach (var posted in postedItems.Where(x => getId(x) == Guid.Empty))
        {
            existingItems.Add(posted);
        }
    }
}
