using HOPS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HOPS.Component;

public sealed class YeastImportService
{
    private readonly HOPSContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<YeastImportService> _logger;

    public YeastImportService(HOPSContext db, IWebHostEnvironment env, ILogger<YeastImportService> logger)
    {
        _db = db;
        _env = env;
        _logger = logger;
    }

    public async Task ImportAllAsync()
    {
        try
        {
            var dataDir = Path.Combine(_env.ContentRootPath, "wwwroot", "data", "yeasts");
            if (!Directory.Exists(dataDir))
            {
                _logger.LogInformation("Yeasts directory not found: {dir}", dataDir);
                return;
            }

            var files = Directory.GetFiles(dataDir, "*.json");
            foreach (var file in files)
            {
                try
                {
                    _logger.LogInformation("Importing yeasts from {file}", file);
                    var text = await File.ReadAllTextAsync(file);
                    using var doc = JsonDocument.Parse(text);
                    if (!doc.RootElement.TryGetProperty("beerjson", out var beerjson))
                    {
                        _logger.LogWarning("File {file} does not contain beerjson root", file);
                        continue;
                    }
                    if (!beerjson.TryGetProperty("cultures", out var cultures) || cultures.ValueKind != JsonValueKind.Array)
                    {
                        _logger.LogWarning("No cultures array in {file}", file);
                        continue;
                    }

                    foreach (var cultureElem in cultures.EnumerateArray())
                    {
                        var productId = cultureElem.GetPropertyOrNull("product_id");
                        if (string.IsNullOrWhiteSpace(productId))
                            continue;

                        var yeast = await _db.Set<Yeast>().FindAsync(productId);
                        if (yeast == null)
                        {
                            yeast = new Yeast { ProductId = productId };
                            _db.Set<Yeast>().Add(yeast);
                        }

                        yeast.Producer = cultureElem.GetPropertyOrNull("producer");
                        yeast.Name = cultureElem.GetPropertyOrNull("name");
                        yeast.Type = cultureElem.GetPropertyOrNull("type");
                        yeast.Notes = cultureElem.GetPropertyOrNull("notes");
                        yeast.BestFor = cultureElem.GetPropertyOrNull("best_for");
                        yeast.Flocculation = cultureElem.GetPropertyOrNull("flocculation");
                        yeast.Form = cultureElem.GetPropertyOrNull("form");

                        yeast.AttenuationRangeMin = cultureElem.GetNestedDecimal("attenuation_range", "minimum");
                        yeast.AttenuationRangeMax = cultureElem.GetNestedDecimal("attenuation_range", "maximum");

                        yeast.TemperatureRangeMin = cultureElem.GetNestedDecimal("temperature_range", "minimum");
                        yeast.TemperatureRangeMax = cultureElem.GetNestedDecimal("temperature_range", "maximum");

                        // alcohol_tolerance typically has a value field
                        yeast.AlcoholTolerance = cultureElem.GetNestedDecimal("alcohol_tolerance", "value");
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
                    _logger.LogError(ex, "Error importing yeasts from {file}", file);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scanning yeasts");
        }
    }
}
