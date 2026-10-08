using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public class FermentationAddition : Addition
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Time))]
        public string Time { get; set; } = string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="recipeBatchSizeLiters"></param>
        /// <param name="targetBatchSizeLiters"></param>
        /// <returns></returns>
        public decimal CalculateAmount(decimal recipeBatchSizeLiters, decimal targetBatchSizeLiters)
        {
            return Amount * (targetBatchSizeLiters / recipeBatchSizeLiters);
        }

        public FermentationAddition CreateCopy()
        {
            return new FermentationAddition
            {
                Index = Index,
                Name = Name,
                Amount = Amount,
                Unit = Unit,
                Time = Time
            };
        }
    }
}