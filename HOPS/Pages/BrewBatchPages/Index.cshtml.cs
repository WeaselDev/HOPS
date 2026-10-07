using HOPS.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Pages.BrewBatchPages;

public class BrewBatchJournalItem
{
    public BrewBatch BrewBatch { get; set; } = null!;

    public BrewRecipeVersion BrewRecipeVersion { get; set; } = null!;

    public BrewRecipe BrewRecipe { get; set; } = null!;
}

public class IndexModel : PageModel
{
    private readonly HOPSContext _context;

    public IndexModel(HOPSContext context)
    {
        _context = context;
    }

    public IList<BrewBatchJournalItem> BrewBatches { get; set; } = [];

    public async Task OnGetAsync()
    {
        BrewBatches = await (
            from batch in _context.BrewBatch
            join version in _context.BrewRecipeVersion
                on batch.BrewRecipeVersionId equals version.Id
            join recipe in _context.BrewRecipe
                on version.BrewRecipeId equals recipe.Id
            where !batch.IsDeleted
            orderby batch.BrewDate descending
            select new BrewBatchJournalItem
            {
                BrewBatch = batch,
                BrewRecipeVersion = version,
                BrewRecipe = recipe
            })
            .ToListAsync();
    }
}