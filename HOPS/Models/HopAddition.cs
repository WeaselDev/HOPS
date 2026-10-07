using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public class HopAddition : Addition
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.AlphaAcid))]
        public decimal AlphaAcid { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Duration))]
        public int Duration { get; set; }

        /// <summary>
        /// Calculates the amount of hops needed based on the batch size, norm factor, and alpha acid percentage.
        /// </summary>
        /// <param name="batchSizeLiters"></param>
        /// <returns></returns>
        public decimal CalculateAmount(decimal recipeBatchSizeLiters, decimal targetBatchSizeLiters, decimal recipeAlphaAcid, decimal targetAlphaAcid)
        {
            return Amount * (targetBatchSizeLiters / recipeBatchSizeLiters) * (recipeAlphaAcid / targetAlphaAcid);
        }

        public HopAddition CreateCopy()
        {
            return new HopAddition
            {
                Index = Index,
                Name = Name,
                Amount = Amount,
                Unit = Unit,
                AlphaAcid = AlphaAcid,
                Duration = Duration
            };
        }
    }
}