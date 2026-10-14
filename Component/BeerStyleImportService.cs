using HOPS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HOPS.Component;

public sealed class BeerStyleImportService
{
    private readonly HOPSContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<BeerStyleImportService> _logger;

    public BeerStyleImportService(HOPSContext db, IWebHostEnvironment env, ILogger<BeerStyleImportService> logger)
    {
        _db = db;
        _env = env;
        _logger = logger;
    }

    public async Task ImportAllAsync()
    {
        try
        {
            var dataDir = Path.Combine(_env.ContentRootPath, "wwwroot", "data", "beerstyles");
            if (!Directory.Exists(dataDir))
            {
                _logger.LogInformation("Beer styles directory not found: {dir}", dataDir);
                return;
            }

var files = Directory.GetFiles(dataDir, "*.json");
            foreach (var file in files)
            {
                // skip files that already have .done extension (e.g. file.json.done won't match *.json)
                try
                {
                    _logger.LogInformation("Importing beer styles from {file}", file);
                    var text = await File.ReadAllTextAsync(file);
                    using var doc = JsonDocument.Parse(text);
                    if (!doc.RootElement.TryGetProperty("beerjson", out var beerjson))
                    {
                        _logger.LogWarning("File {file} does not contain beerjson root", file);
                        continue;
                    }
                    if (!beerjson.TryGetProperty("styles", out var styles) || styles.ValueKind != JsonValueKind.Array)
                    {
                        _logger.LogWarning("No styles array in {file}", file);
                        continue;
                    }

                    foreach (var styleElem in styles.EnumerateArray())
                    {
                        var styleId = styleElem.GetPropertyOrNull("style_id");
                        if (string.IsNullOrWhiteSpace(styleId))
                            continue;

                        var style = await _db.Set<BeerStyle>().FindAsync(styleId);
                        if (style == null)
                        {
                            style = new BeerStyle { StyleId = styleId };
                            _db.Set<BeerStyle>().Add(style);
                        }

                        style.Name = styleElem.GetPropertyOrNull("name");
                        style.Category = styleElem.GetPropertyOrNull("category");
                        style.CategoryId = styleElem.GetPropertyOrNull("category_id");
                        style.CategoryDescription = styleElem.GetPropertyOrNull("category_description");
                        style.OverallImpression = styleElem.GetPropertyOrNull("overall_impression");
                        style.Aroma = styleElem.GetPropertyOrNull("aroma");
                        style.Appearance = styleElem.GetPropertyOrNull("appearance");
                        style.Flavor = styleElem.GetPropertyOrNull("flavor");
                        style.Mouthfeel = styleElem.GetPropertyOrNull("mouthfeel");
                        style.Comments = styleElem.GetPropertyOrNull("comments");
                        style.History = styleElem.GetPropertyOrNull("history");
                        style.StyleComparison = styleElem.GetPropertyOrNull("style_comparison");
                        style.Tags = styleElem.GetPropertyOrNull("tags");
                        style.Ingredients = styleElem.GetPropertyOrNull("ingredients");
                        style.Examples = styleElem.GetPropertyOrNull("examples");
                        style.StyleGuide = styleElem.GetPropertyOrNull("style_guide");
                        style.Type = styleElem.GetPropertyOrNull("type");
                        style.Notes = styleElem.GetPropertyOrNull("notes");

                        style.OgMin = styleElem.GetNestedDecimal("original_gravity", "minimum");
                        style.OgMax = styleElem.GetNestedDecimal("original_gravity", "maximum");
                        style.IbuMin = styleElem.GetNestedDecimal("international_bitterness_units", "minimum");
                        style.IbuMax = styleElem.GetNestedDecimal("international_bitterness_units", "maximum");
                        style.FgMin = styleElem.GetNestedDecimal("final_gravity", "minimum");
                        style.FgMax = styleElem.GetNestedDecimal("final_gravity", "maximum");
                        style.AbvMin = styleElem.GetNestedDecimal("alcohol_by_volume", "minimum");
                        style.AbvMax = styleElem.GetNestedDecimal("alcohol_by_volume", "maximum");
                        style.ColorMin = styleElem.GetNestedDecimal("color", "minimum");
                        style.ColorMax = styleElem.GetNestedDecimal("color", "maximum");
                    }

                    await _db.SaveChangesAsync();

                    // rename file to add .done extension
                    var doneName = file + ".done";
                    if (File.Exists(doneName))
                        File.Delete(doneName);
                    File.Move(file, doneName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error importing styles from {file}", file);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning beer styles");
        }
    }
}

static class JsonExtensions
{
    public static string? GetPropertyOrNull(this JsonElement elem, string name)
    {
        if (elem.ValueKind != JsonValueKind.Object)
            return null;
        if (!elem.TryGetProperty(name, out var p))
            return null;
        if (p.ValueKind == JsonValueKind.Null)
            return null;
        return p.ToString();
    }

public static decimal? GetNestedDecimal(this JsonElement elem, string propName, string nestedName)
    {
        if (elem.ValueKind != JsonValueKind.Object)
            return null;
        if (!elem.TryGetProperty(propName, out var prop))
            return null;
        if (prop.ValueKind != JsonValueKind.Object)
            return null;
        if (!prop.TryGetProperty(nestedName, out var nested))
            return null;
        if (nested.ValueKind != JsonValueKind.Object)
            return null;
        if (!nested.TryGetProperty("value", out var value))
            return null;
        if (value.ValueKind == JsonValueKind.Number)
        {
            if (value.TryGetDecimal(out var dec))
                return dec;
            if (value.TryGetDouble(out var d))
                return (decimal)d;
        }
        if (value.ValueKind == JsonValueKind.String)
        {
            if (decimal.TryParse(value.GetString(), out var dec2))
                return dec2;
        }
        return null;
    }
}
