using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public class BrewRecipeVersion : Model
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Version))]
        public int Version { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Notes))]
        public string Notes { get; set; } = string.Empty;

        public Guid BrewRecipeId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.IsDeleted))]
        public bool IsDeleted { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BatchSize))]
        public int BatchSize { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BrewhouseEfficiency))]
        public decimal BrewhouseEfficiency { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.OriginalGravity))]
        public decimal OriginalGravity { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Bitterness))]
        public int Bitterness { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Color))]
        public decimal Color { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.EstimatedABV))]
        public decimal EstimatedABV { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.ApparentAttenuation))]
        public decimal ApparantAttenuation { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.FermentationTemperature))]
        public int FermentationTemperature { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Carbonation))]
        public decimal Carbonation { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.TotalWater))]
        public decimal TotalWater { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.MashWaterPercentage))]
        public decimal MashWaterPercentage { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.BoilDuration))]
        public int BoilDuration { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UseIrishMoss))]
        public bool UseIrishMoss { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UseWhirlfloc))]
        public bool UseWhirlfloc { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.UseSilicaSol))]
        public bool UseSilicaSol { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.MaturationWeeksMin))]
        public int MaturationWeeksMin { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.MaturationWeeksMax))]
        public int MaturationWeeksMax { get; set; }

        public List<MaltAddition> MaltAdditions { get; set; } = [];
        public List<HopAddition> HopAdditions { get; set; } = [];
        public List<YeastAddition> YeastAdditions { get; set; } = [];
        public List<MashStep> MashSteps { get; set; } = [];
        public List<FermentationAddition> FermentationAdditions { get; set; } = [];
        public List<WaterAddition> WaterAdditions { get; set; } = [];

        public BrewRecipeVersion CreateNewVersion(int version)
        {
            return new BrewRecipeVersion
            {
                Version = version,
                BrewRecipeId = BrewRecipeId,

                Notes = Notes,

                BatchSize = BatchSize,
                BrewhouseEfficiency = BrewhouseEfficiency,
                OriginalGravity = OriginalGravity,
                Bitterness = Bitterness,
                Color = Color,
                EstimatedABV = EstimatedABV,
                ApparantAttenuation = ApparantAttenuation,
                FermentationTemperature = FermentationTemperature,
                Carbonation = Carbonation,

                TotalWater = TotalWater,
                MashWaterPercentage = MashWaterPercentage,

                BoilDuration = BoilDuration,

                UseIrishMoss = UseIrishMoss,
                UseWhirlfloc = UseWhirlfloc,
                UseSilicaSol = UseSilicaSol,

                MaltAdditions = [.. MaltAdditions.Select(a => a.CreateCopy())],
                HopAdditions = [.. HopAdditions.Select(a => a.CreateCopy())],
                YeastAdditions = [.. YeastAdditions.Select(a => a.CreateCopy())],
                MashSteps = [.. MashSteps.Select(a => a.CreateCopy())],
                FermentationAdditions = [.. FermentationAdditions.Select(a => a.CreateCopy())],
                WaterAdditions = [.. WaterAdditions.Select(a => a.CreateCopy())]
            };
        }
    }
}