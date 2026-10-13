using System.Text.Json;
using System.Text.Json.Serialization;

namespace HOPS.Component;

public sealed class BjcpDocument
{
    [JsonPropertyName("beerjson")]
    public BjcpBeerJsonDto? BeerJson { get; set; }
}

public sealed class BjcpBeerJsonDto
{
    [JsonPropertyName("version")]
    public decimal Version { get; set; }

    [JsonPropertyName("styles")]
    public List<BjcpStyleDto> Styles { get; set; } = new();
}

public sealed class BjcpStyleDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("category_id")]
    public string? CategoryId { get; set; }

    [JsonPropertyName("style_id")]
    public string? StyleId { get; set; }

    [JsonPropertyName("category_description")]
    public string? CategoryDescription { get; set; }

    [JsonPropertyName("overall_impression")]
    public string? OverallImpression { get; set; }

    [JsonPropertyName("aroma")]
    public string? Aroma { get; set; }

    [JsonPropertyName("appearance")]
    public string? Appearance { get; set; }

    [JsonPropertyName("flavor")]
    public string? Flavor { get; set; }

    [JsonPropertyName("mouthfeel")]
    public string? Mouthfeel { get; set; }

    [JsonPropertyName("comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("history")]
    public string? History { get; set; }

    [JsonPropertyName("style_comparison")]
    public string? StyleComparison { get; set; }

    [JsonPropertyName("tags")]
    public string? Tags { get; set; }

    [JsonPropertyName("ingredients")]
    public string? Ingredients { get; set; }

    [JsonPropertyName("examples")]
    public string? Examples { get; set; }

    [JsonPropertyName("style_guide")]
    public string? StyleGuide { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("original_gravity")]
    public BjcpMeasurementRangeDto? OriginalGravity { get; set; }

    [JsonPropertyName("international_bitterness_units")]
    public BjcpMeasurementRangeDto? InternationalBitternessUnits { get; set; }

    [JsonPropertyName("final_gravity")]
    public BjcpMeasurementRangeDto? FinalGravity { get; set; }

    [JsonPropertyName("alcohol_by_volume")]
    public BjcpMeasurementRangeDto? AlcoholByVolume { get; set; }

    [JsonPropertyName("color")]
    public BjcpMeasurementRangeDto? Color { get; set; }

    // Bewahrt zusätzliche lokalisierte oder zukünftige JSON-Felder beim Deserialisieren.
    // Sie werden durch den unten gezeigten Importservice nicht in BeerStyle gespeichert.
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}

public sealed class BjcpMeasurementRangeDto
{
    [JsonPropertyName("minimum")]
    public BjcpMeasurementDto? Minimum { get; set; }

    [JsonPropertyName("maximum")]
    public BjcpMeasurementDto? Maximum { get; set; }
}

public sealed class BjcpMeasurementDto
{
    [JsonPropertyName("unit")]
    public string? Unit { get; set; }

    [JsonPropertyName("value")]
    public decimal Value { get; set; }
}