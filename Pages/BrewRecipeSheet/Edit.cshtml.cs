using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HOPS.Models;

namespace HOPS.Pages.BrewRecipeSheet;

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

    public List<SelectListItem> BeerStyleList { get; set; } = new List<SelectListItem>();

    public List<SelectListItem> PreferredYeastList { get; set; } = new List<SelectListItem>();

        // Full yeast objects for rendering data-* attributes (producer/name)
        public List<HOPS.Models.Yeast> PreferredYeastData { get; set; } = new List<HOPS.Models.Yeast>();

    public async Task<IActionResult> OnGetAsync(System.Guid? id, int? version)
    {
        if (id is null)
        {
            return NotFound();
        }

        var brewrecipe = await _context.BrewRecipe
            .Include(r => r.BeerStyle)
            .Include(r => r.Versions)
            .ThenInclude(v => v.MashSteps.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.HopAdditions.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.MaltAdditions.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.PreferredYeasts)
            .Include(r => r.Versions)
            .ThenInclude(v => v.FermentationAdditions.OrderBy(x => x.Index))
            .Include(r => r.Versions)
            .ThenInclude(v => v.WaterAdditions.OrderBy(x => x.Index))
            .FirstOrDefaultAsync(r => r.Id == id);

        if (brewrecipe is null)
        {
            return NotFound();
        }

        if (brewrecipe.IsDeleted)
        {
            return NotFound();
        }

        if (version.HasValue)
        {
            BrewRecipeVersion = brewrecipe.Versions
                .FirstOrDefault(v => v.Version == version.Value)!;

            if (BrewRecipeVersion is null)
            {
                return NotFound();
            }
        }
        else
        {
            BrewRecipeVersion = brewrecipe.Versions
                .Where(v => !v.IsDeleted)
                .OrderByDescending(v => v.Version)
                .FirstOrDefault()!;

            if (BrewRecipeVersion is null)
            {
                return NotFound();
            }
        }

        BrewRecipe = brewrecipe;
        // populate preferred yeast ids for the UI multi-select
        BrewRecipeVersion.PreferredYeastIds = [.. brewrecipe.Versions
            .Where(v => v.Id == BrewRecipeVersion.Id)
            .SelectMany(v => v.PreferredYeasts.Select(y => y.ProductId))];

        await LoadBeerStylesAsync();
        await LoadPreferredYeastsAsync();
        return Page();
    }

    private async Task LoadBeerStylesAsync()
    {
        var styles = await _context.BeerStyles
            .OrderBy(b => b.Category)
            .ThenBy(b => b.Name)
            .ToListAsync();

        var groups = new Dictionary<string, SelectListGroup>();
        var list = new List<SelectListItem>();

        foreach (var s in styles)
        {
            var key = string.IsNullOrWhiteSpace(s.Category) ? "Uncategorized" : s.Category!;
            if (!groups.TryGetValue(key, out var grp))
            {
                grp = new SelectListGroup { Name = key };
                groups[key] = grp;
            }

            list.Add(new SelectListItem
            {
                Value = s.StyleId,
                Text = (s.Name ?? s.Category ?? s.StyleId),
                Group = grp
            });
        }

        BeerStyleList = list;
    }

    private async Task LoadPreferredYeastsAsync()
    {
        var yeasts = await _context.Yeasts
            .OrderBy(y => y.Producer)
            .ThenBy(y => y.Name)
            .ToListAsync();

        PreferredYeastData = yeasts;

        var list = new List<SelectListItem>();

        foreach (var y in yeasts)
        {
            list.Add(new SelectListItem
            {
                Value = y.ProductId,
                Text = (y.Producer is null ? "" : y.Producer + " - ") + (y.Name ?? y.ProductId)
            });
        }

        PreferredYeastList = list;
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadBeerStylesAsync();
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
                .ThenInclude(v => v.PreferredYeasts)
            .Include(r => r.Versions)
                .ThenInclude(v => v.FermentationAdditions)
            .Include(r => r.Versions)
                .ThenInclude(v => v.WaterAdditions)
            .FirstOrDefaultAsync(r => r.Id == BrewRecipe.Id);

        if (brewRecipe == null)
        {
            return NotFound();
        }

        if (brewRecipe.IsDeleted)
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
        brewRecipe.BeerStyleId = BrewRecipe.BeerStyleId;
        brewRecipe.Description = BrewRecipe.Description;

        BrewRecipeVersion.MaturationWeeksMax = Math.Max(BrewRecipeVersion.MaturationWeeksMin, BrewRecipeVersion.MaturationWeeksMax);

        bool versionWasBrewed = await _context.BrewBatch
            .AnyAsync(b => b.BrewRecipeVersionId == recipeVersion.Id);

        if (recipeVersion.IsDeleted || versionWasBrewed)
        {
            CreateNewVersion(brewRecipe);

            // assign preferred yeasts to the newly created version
            var newVersion = brewRecipe.Versions.OrderByDescending(v => v.Version).First();
            newVersion.PreferredYeasts = await _context.Yeasts
                .Where(y => BrewRecipeVersion.PreferredYeastIds.Contains(y.ProductId))
                .ToListAsync();
        }
        else
        {
            UpdateVersion(recipeVersion);

            // update preferred yeasts for existing version
            recipeVersion.PreferredYeasts = await _context.Yeasts
                .Where(y => BrewRecipeVersion.PreferredYeastIds.Contains(y.ProductId))
                .ToListAsync();
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
            PreferredYeasts = [.. BrewRecipeVersion.PreferredYeasts],
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
