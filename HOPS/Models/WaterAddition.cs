using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public enum WaterAdditionType
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.DestilledWater))]
        DistilledWater,

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.LacticAcid))]
        LacticAcid,

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.CalciumChloride))]
        CalciumChloride,

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.CalciumSulfate))]
        CalciumSulfate,

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.MagnesiumSulfate))]
        MagnesiumSulfate,
    }

    public class WaterAddition : Addition
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Addition))]
        public WaterAdditionType Type { get; set; }

        public decimal CalculateAmount(decimal recipeBatchSizeLiters, decimal targetBatchSizeLiters)
        {
            return Amount * (targetBatchSizeLiters / recipeBatchSizeLiters);
        }

        public WaterAddition CreateCopy()
        {
            return new WaterAddition
            {
                Index = Index,
                Name = Name,
                Amount = Amount,
                Unit = Unit,
                Type = Type
            };
        }
    }
}