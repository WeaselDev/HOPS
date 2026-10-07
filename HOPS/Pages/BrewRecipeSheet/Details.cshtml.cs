using HOPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Pages.BrewRecipeSheet;

public class DetailsModel : PageModel
{
    private readonly HOPSContext _context;

    public DetailsModel(HOPSContext context)
    {
        _context = context;
    }

    public BrewRecipe BrewRecipe { get; set; } = default!;

    public BrewRecipeVersion BrewRecipeVersion { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id, int? version)
    {
        if (id is null)
        {
            return NotFound();
        }

        BrewRecipe = await _context.BrewRecipe
            .Include(r => r.Versions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (BrewRecipe is null)
        {
            return NotFound();
        }

        int versionNumber;

        if (version.HasValue)
        {
            versionNumber = version.Value;
        }
        else
        {
            var currentVersion = BrewRecipe.Versions
                .Where(v => !v.IsDeleted)
                .OrderByDescending(v => v.Version)
                .FirstOrDefault();

            if (currentVersion is null)
            {
                return NotFound();
            }

            versionNumber = currentVersion.Version;
        }

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
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteVersionAsync(
        Guid id,
        int version)
    {
        var brewRecipe = await _context.BrewRecipe
            .Include(r => r.Versions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (brewRecipe is null || brewRecipe.IsDeleted)
        {
            return NotFound();
        }

        var recipeVersion = brewRecipe.Versions
            .FirstOrDefault(v => v.Version == version);

        if (recipeVersion is null)
        {
            return NotFound();
        }

        recipeVersion.IsDeleted = true;

        await _context.SaveChangesAsync();

        var nextVersion = brewRecipe.Versions
            .Where(v => !v.IsDeleted)
            .OrderByDescending(v => v.Version)
            .FirstOrDefault();

        if (nextVersion is null)
        {
            return RedirectToPage("./Index");
        }

        return RedirectToPage(
            "./Details",
            new
            {
                id = brewRecipe.Id,
                version = nextVersion.Version
            });
    }

    public async Task<IActionResult> OnPostDeleteRecipeAsync(Guid id)
    {
        var brewRecipe = await _context.BrewRecipe
            .Include(r => r.Versions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (brewRecipe is null)
        {
            return NotFound();
        }

        brewRecipe.IsDeleted = true;

        foreach (var version in brewRecipe.Versions)
        {
            version.IsDeleted = true;
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}