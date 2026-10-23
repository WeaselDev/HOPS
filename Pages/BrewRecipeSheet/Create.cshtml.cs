using HOPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Pages.BrewRecipeSheet;

public class CreateModel : PageModel
{
    private readonly HOPSContext _context;

    public CreateModel(HOPSContext context)
    {
        _context = context;

        BrewRecipe = new BrewRecipe();

        var version = new BrewRecipeVersion
        {
            Version = 1,
            MashWaterPercentage = 75,

            MashSteps =
            [
                new MashStep
                {
                    Index = 1,
                    Name = "Einmaischen",
                    Temperature = 57,
                    Duration = 0
                },
                new MashStep
                {
                    Index = 2,
                    Name = "1. Rast",
                    Temperature = 0,
                    Duration = 0
                },
                new MashStep
                {
                    Index = 3,
                    Name = "2. Rast",
                    Temperature = 0,
                    Duration = 0
                },
                new MashStep
                {
                    Index = 4,
                    Name = "Abmaischen",
                    Temperature = 78,
                    Duration = 5
                }
            ]
        };

        BrewRecipe.Versions.Add(version);
    }

    [BindProperty]
    public BrewRecipe BrewRecipe { get; set; } = new();

    public List<SelectListItem> BeerStyleList { get; set; } = new List<SelectListItem>();

    public List<SelectListItem> PreferredYeastList { get; set; } = new List<SelectListItem>();

    // Full yeast objects for rendering data-* attributes (producer/name)
    public List<HOPS.Models.Yeast> PreferredYeastData { get; set; } = new List<HOPS.Models.Yeast>();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadBeerStylesAsync();
        await LoadPreferredYeastsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadBeerStylesAsync();
            return Page();
        }

        BrewRecipeVersion version = BrewRecipe.Versions.FirstOrDefault();
        version.MaturationWeeksMax = Math.Max(version.MaturationWeeksMin, version.MaturationWeeksMax);

        // attach preferred yeasts selected in the UI
        if (version.PreferredYeastIds != null && version.PreferredYeastIds.Any())
        {
            version.PreferredYeasts = await _context.Yeasts
                .Where(y => version.PreferredYeastIds.Contains(y.ProductId))
                .ToListAsync();
        }

        _context.BrewRecipe.Add(BrewRecipe);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
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
                Text = s.GetName("de"),
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
}