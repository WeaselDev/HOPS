using System.ComponentModel.DataAnnotations;

namespace HOPS.Models;

public class Yeast
{
    [Key]
    [MaxLength(100)]
    public string ProductId { get; set; } = null!; // use product_id as primary key

    [MaxLength(200)]
    public string? Producer { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(50)]
    public string? Type { get; set; }

    public string? Notes { get; set; }

    public string? BestFor { get; set; }

    public decimal? AttenuationRangeMin { get; set; }
    public decimal? AttenuationRangeMax { get; set; }

    public string? Flocculation { get; set; }

    public decimal? TemperatureRangeMin { get; set; }
    public decimal? TemperatureRangeMax { get; set; }

    public decimal? AlcoholTolerance { get; set; }

    public string? Form { get; set; }
}
