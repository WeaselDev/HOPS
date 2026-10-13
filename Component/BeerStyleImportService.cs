using HOPS.Models;
using Microsoft.EntityFrameworkCore;

namespace HOPS.Component;

public sealed class BeerStyleImportService
{
    private readonly HOPSContext _dbContext;

    public BeerStyleImportService(HOPSContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> ImportAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        var document = await BjcpImporter.LoadAsync(path, cancellationToken);

        var importedStyles = document.BeerJson?.Styles ?? new List<BjcpStyleDto>();

        // Ungültige IDs überspringen. Bei doppelten IDs in der JSON-Datei
        // wird der jeweils letzte Eintrag verwendet.
        var uniqueImportedStyles = importedStyles
            .Where(style => !string.IsNullOrWhiteSpace(style.StyleId))
            .GroupBy(style => style.StyleId!)
            .Select(group => group.Last())
            .ToList();

        var styleIds = uniqueImportedStyles
            .Select(style => style.StyleId!)
            .ToList();

        var existingStyles = await _dbContext.BeerStyles
            .Where(style => styleIds.Contains(style.StyleId))
            .ToDictionaryAsync(style => style.StyleId, cancellationToken);

        foreach (var imported in uniqueImportedStyles)
        {
            if (existingStyles.TryGetValue(imported.StyleId!, out var existing))
            {
                ApplyImport(existing, imported);
            }
            else
            {
                var newStyle = new BeerStyle();
                ApplyImport(newStyle, imported);
                _dbContext.BeerStyles.Add(newStyle);
            }
        }

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static void ApplyImport(BeerStyle entity, BjcpStyleDto imported)
    {
        entity.StyleId = imported.StyleId!;
        entity.Name = imported.Name;
        entity.Category = imported.Category;
        entity.CategoryId = imported.CategoryId;
        entity.CategoryDescription = imported.CategoryDescription;
        entity.OverallImpression = imported.OverallImpression;
        entity.Aroma = imported.Aroma;
        entity.Appearance = imported.Appearance;
        entity.Flavor = imported.Flavor;
        entity.Mouthfeel = imported.Mouthfeel;
        entity.Comments = imported.Comments;
        entity.History = imported.History;
        entity.StyleComparison = imported.StyleComparison;
        entity.Tags = imported.Tags;
        entity.Ingredients = imported.Ingredients;
        entity.Examples = imported.Examples;
        entity.StyleGuide = imported.StyleGuide;
        entity.Type = imported.Type;
        entity.Notes = imported.Notes;

        entity.OgMin = imported.OriginalGravity?.Minimum?.Value;
        entity.OgMax = imported.OriginalGravity?.Maximum?.Value;

        entity.IbuMin = imported.InternationalBitternessUnits?.Minimum?.Value;
        entity.IbuMax = imported.InternationalBitternessUnits?.Maximum?.Value;

        entity.FgMin = imported.FinalGravity?.Minimum?.Value;
        entity.FgMax = imported.FinalGravity?.Maximum?.Value;

        entity.AbvMin = imported.AlcoholByVolume?.Minimum?.Value;
        entity.AbvMax = imported.AlcoholByVolume?.Maximum?.Value;

        entity.ColorMin = imported.Color?.Minimum?.Value;
        entity.ColorMax = imported.Color?.Maximum?.Value;
    }
}