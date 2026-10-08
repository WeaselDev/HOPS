using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOPS.Models;

namespace HOPS.Pages.BrewRecipePages;

public class DetailsModel : PageModel
{
    private readonly HOPSContext _context;
    public DetailsModel(HOPSContext context)
    {
        _context = context;
    }

    public BrewRecipe BrewRecipe { get; set; } = default!;

    public BrewRecipeVersion BrewRecipeVersion { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(System.Guid? id, int? version)
    {
        if (id is null)
        {
            return NotFound();
        }

        BrewRecipe = await _context.BrewRecipe
            .Include(r => r.Versions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (BrewRecipe == null)
            return NotFound();

        var versionNumber = version ?? BrewRecipe.Versions.Max(v => v.Version);

        BrewRecipeVersion = await _context.BrewRecipeVersion
            .Include(v => v.MaltAdditions.OrderBy(x => x.Index))
            .Include(v => v.HopAdditions.OrderBy(x => x.Index))
            .Include(v => v.YeastAdditions.OrderBy(x => x.Index))
            .Include(v => v.MashSteps.OrderBy(x => x.Index))
            .Include(v => v.FermentationAdditions.OrderBy(x => x.Index))
            .Include(v => v.WaterAdditions.OrderBy(x => x.Index))
            .FirstOrDefaultAsync(v =>
            v.BrewRecipeId == id &&
            v.Version == versionNumber);

        if (BrewRecipeVersion is null)
            return NotFound();

        return Page();
    }
}
