using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOPS.Models;

namespace HOPS.Pages.BrewBatchPages;

public class DeleteModel : PageModel
{
    private readonly HOPSContext _context;

    public DeleteModel(HOPSContext context)
    {
        _context = context;
    }

    [BindProperty]
    public BrewBatch BrewBatch { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(System.Guid? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var brewbatch = await _context.BrewBatch.FirstOrDefaultAsync(m => m.Id == id);
        if (brewbatch is null)
        {
            return NotFound();
        }
        else
        {
            BrewBatch = brewbatch;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(System.Guid? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var brewbatch = await _context.BrewBatch.FindAsync(id);
        if (brewbatch != null)
        {
            BrewBatch = brewbatch;
            _context.BrewBatch.Remove(BrewBatch);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
