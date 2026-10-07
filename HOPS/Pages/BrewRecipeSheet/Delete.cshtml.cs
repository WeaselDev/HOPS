using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOPS.Models;

namespace HOPS.Pages.BrewRecipeSheet;

public class DeleteModel : PageModel
{
    private readonly HOPSContext _context;

    public DeleteModel(HOPSContext context)
    {
        _context = context;
    }

    [BindProperty]
    public BrewRecipe BrewRecipe { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(System.Guid? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var brewrecipe = await _context.BrewRecipe
            .Include(b => b.Versions)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (brewrecipe is null)
        {
            return NotFound();
        }
        else
        {
            BrewRecipe = brewrecipe;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(System.Guid? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var brewrecipe = await _context.BrewRecipe
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
            .FirstOrDefaultAsync(r => r.Id == id);

        if (brewrecipe != null)
        {
            BrewRecipe = brewrecipe;
            _context.BrewRecipe.Remove(BrewRecipe);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
