using System.ComponentModel.DataAnnotations;

namespace HOPS.Models;

public class BeerStyle
{
    [Key]
    [MaxLength(10)]
    public string StyleId { get; set; } = null!;

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(200)]
    public string? Category { get; set; }

    [MaxLength(20)]
    public string? CategoryId { get; set; }

    public string? CategoryDescription { get; set; }
    public string? OverallImpression { get; set; }
    public string? Aroma { get; set; }
    public string? Appearance { get; set; }
    public string? Flavor { get; set; }
    public string? Mouthfeel { get; set; }
    public string? Comments { get; set; }
    public string? History { get; set; }
    public string? StyleComparison { get; set; }
    public string? Tags { get; set; }
    public string? Ingredients { get; set; }
    public string? Examples { get; set; }
    public string? StyleGuide { get; set; }
    public string? Type { get; set; }
    public string? Notes { get; set; }

    public decimal? OgMin { get; set; }
    public decimal? OgMax { get; set; }
    public decimal? IbuMin { get; set; }
    public decimal? IbuMax { get; set; }
    public decimal? FgMin { get; set; }
    public decimal? FgMax { get; set; }
    public decimal? AbvMin { get; set; }
    public decimal? AbvMax { get; set; }
    public decimal? ColorMin { get; set; }
    public decimal? ColorMax { get; set; }
}
