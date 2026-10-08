using HOPS.Component;
using HOPS.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HOPS.Models
{
    public enum BrewBatchStatus
    {
        Planned,
        Ready,
        Brewed,
        Bottled
    }

    public class BrewBatch : Model
    {
        /// <summary>
        /// The ID of the brew recipe version that this batch is based on.
        /// </summary>
        public Guid BrewRecipeVersionId { get; set; }

        /// <summary>
        /// The brew recipe version that this batch is based on.
        /// </summary>
        public BrewRecipeVersion? BrewRecipeVersion { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Status))]
        public BrewBatchStatus Status { get; set; } = BrewBatchStatus.Planned;

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.IsDeleted))]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The size of the batch in liters.
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BatchSize))]
        public int BatchSize { get; set; }

        /// <summary>
        /// The brewhouse efficiency as a percentage (0-100).
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BrewhouseEfficiency))] 
        public decimal BrewhouseEfficiency { get; set; }

        /// <summary>
        /// The mash water percentage as a percentage (0-100).
        /// </summary>
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.MashWaterPercentage))]
        public decimal MashWaterPercentage { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UseIrishMoss))]
        public bool UseIrishMoss { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UseWhirlfloc))]
        public bool UseWhirlfloc { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UseSilicaSol))]
        public bool UseSilicaSol { get; set; }

        /// <summary>
        /// The list of alphas for hop additions for this batch.
        /// </summary>
        public List<BrewHop> Hops { get; set; } = [];

        /// <summary>
        /// The list of water additions for this batch.
        /// </summary>
        public List<WaterAddition> WaterAdditions { get; set; } = [];

        /// <summary>
        /// The list of yeast additions for this batch.
        /// </summary>
        public List<YeastAddition> YeastAdditions { get; set; } = [];

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BrewNote))]
        public string? BrewNote { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BrewDate))]
        public DateTime? BrewDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.FermentationStartDate))]
        public DateTime? FermentationStartDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.FermentationStartPlato))]
        public decimal? FermentationStartPlato { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BottlingDate))]
        public DateTime? BottlingDate { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BottlingPlato))]
        public decimal? BottlingPlato { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BottlingTemperature))]
        public decimal? BottlingTemperature { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BottlingVolume))]
        public decimal? BottlingVolume { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.ApparentAttenuation))]
        public decimal? ApparentAttenuation { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.ActualABV))]
        public decimal? ActualABV { get; set; }

        [Range(0, 5)]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Rating))]
        public int? Rating { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.RatingNote))]
        public string? RatingNote { get; set; }


        [NotMapped]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.ResidualCo2))]
        public decimal? ResidualCo2
        {
            get
            {
                if (!BottlingTemperature.HasValue)
                {
                    return null;
                }

                return CarbonationCalculator.CalculateResidualCo2(
                BottlingTemperature.Value);
            }
        }

        [NotMapped]
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.PrimingSugar))]
        public decimal? PrimingSugar
        {
            get
            {
                if (
                !BottlingVolume.HasValue ||
                !BottlingTemperature.HasValue ||
                BrewRecipeVersion is null)
                {
                    return null;
                }

                return CarbonationCalculator.CalculateSucrose(
                BottlingVolume.Value,
                BottlingTemperature.Value,
                BrewRecipeVersion.Carbonation);
            }
        }
    }
}