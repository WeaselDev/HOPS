namespace HOPS.Models
{
    public class MaltAddition : Addition
    {
        public decimal CalculateAmount(decimal recipeBatchSizeLiters, decimal targetBatchSizeLiters, decimal recipeBrewhouseEfficiency, decimal targetBrewhouseEfficiency)
        {
            return (Amount * (targetBatchSizeLiters / recipeBatchSizeLiters) * (recipeBrewhouseEfficiency / targetBrewhouseEfficiency));
        }

        public MaltAddition CreateCopy()
        {
            return new MaltAddition
            {
                Index = Index,
                Name = Name,
                Amount = Amount,
                Unit = Unit,
            };
        }
    }
}