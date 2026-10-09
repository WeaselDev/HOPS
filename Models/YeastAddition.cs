using HOPS.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HOPS.Models
{
    public class YeastAddition : Addition
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Description))]
        public string Description { get; set; } = string.Empty;

        [NotMapped]
        public bool Selected { get; set; }

        public decimal CalculateAmount(decimal recipeBatchSizeLiters, decimal targetBatchSizeLiters)
        {
            int hackedYeastRange = 25;
            return Math.Ceiling(targetBatchSizeLiters / hackedYeastRange);
        }

        public YeastAddition CreateCopy()
        {
            return new YeastAddition
            {
                Index = Index,
                Name = Name,
                Amount = Amount,
                Unit = Unit,
                Description = Description
            };
        }
    }
}