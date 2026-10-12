using System.ComponentModel.DataAnnotations.Schema;

namespace HOPS.Models
{
    public class BeerStyle : NamedModel
    {

        // BeerJSON / BJCP Referenz
        public string StyleId { get; set; } = string.Empty;

        // BJCP-Klassifizierung
        public string? Category { get; set; }
        public string? CategoryNumber { get; set; }
        public string? StyleLetter { get; set; }

        // Beschreibung
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string? Aroma { get; set; }
        public string? Appearance { get; set; }
        public string? Flavor { get; set; }
        public string? Mouthfeel { get; set; }
        public string? Ingredients { get; set; }
        public string? History { get; set; }
        public string? Examples { get; set; }

        // Zielwerte
        public decimal? OgMin { get; set; }
        public decimal? OgMax { get; set; }

        public decimal? FgMin { get; set; }
        public decimal? FgMax { get; set; }

        public decimal? IbuMin { get; set; }
        public decimal? IbuMax { get; set; }

        public decimal? SrmMin { get; set; }
        public decimal? SrmMax { get; set; }

        public decimal? AbvMin { get; set; }
        public decimal? AbvMax { get; set; }

        // Metadaten
        public bool IsImported { get; set; }

        public string Source { get; set; } = "Manual";

        public DateTime CreatedAt { get; set; }

        public DateTime? LastImportedAt { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        [NotMapped]
        public string DisplayCode
        {
            get => $"{CategoryNumber}{StyleLetter}";
        }
    }
}
