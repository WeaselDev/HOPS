namespace HOPS.Component
{
    public static class CarbonationCalculator
    {
        private const decimal Co2GramsPerLiterPerVolume = 1.977m;
        private const decimal Co2GramsPerGramSucrose = 0.514m;

        public static decimal CalculateResidualCo2(decimal temperatureCelsius)
        {
            var temperatureFahrenheit =
                temperatureCelsius * 9m / 5m + 32m;

            var volumes =
                3.0378m
                - 0.050062m * temperatureFahrenheit
                + 0.00026555m * temperatureFahrenheit * temperatureFahrenheit;

            return Math.Max(
                0m,
                volumes * Co2GramsPerLiterPerVolume);
        }

        public static decimal CalculateSucrose(
            decimal volumeLiters,
            decimal temperatureCelsius,
            decimal targetCo2GramsPerLiter)
        {
            if (volumeLiters <= 0)
            {
                return 0;
            }

            var residualCo2 =
                CalculateResidualCo2(temperatureCelsius);

            var requiredCo2PerLiter = Math.Max(
                0m,
                targetCo2GramsPerLiter - residualCo2);

            var requiredCo2 =
                requiredCo2PerLiter * volumeLiters;

            return requiredCo2 / Co2GramsPerGramSucrose;
        }
    }
}