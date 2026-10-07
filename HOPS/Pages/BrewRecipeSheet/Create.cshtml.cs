using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        BrewRecipeVersion version = BrewRecipe.Versions.FirstOrDefault();
        version.MaturationWeeksMax = Math.Max(version.MaturationWeeksMin, version.MaturationWeeksMax);

        _context.BrewRecipe.Add(BrewRecipe);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}