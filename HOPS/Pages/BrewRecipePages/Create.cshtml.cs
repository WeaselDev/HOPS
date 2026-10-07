using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOPS.Models;

namespace HOPS.Pages.BrewRecipePages;

public class CreateModel : PageModel
{
    private readonly HOPSContext _context;

    public CreateModel(HOPSContext context)
    {
        BrewRecipe = new BrewRecipe();
        BrewRecipeVersion v = new BrewRecipeVersion();
        v.MashSteps.Add(new MashStep());
        v.MaltAdditions.Add(new MaltAddition());
        v.HopAdditions.Add(new HopAddition());
        v.YeastAdditions.Add(new YeastAddition());
        v.FermentationAdditions.Add(new FermentationAddition());
        //v.WaterAdditions.Add(new WaterAddition());
        v.Version = 1;
        BrewRecipe.Versions.Add(v);
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public BrewRecipe BrewRecipe { get; set; } = new();

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        //var version = BrewRecipe.Versions[0];

        _context.BrewRecipe.Add(BrewRecipe);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}