using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HOPS.Models;

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

    public List<SelectListItem> BeerStyleList { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadBeerStylesAsync();
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
                Text = (s.Name ?? s.Category ?? s.StyleId),
                Group = grp
            });
        }

        BeerStyleList = list;
    }
}
