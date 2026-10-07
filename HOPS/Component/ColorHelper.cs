namespace HOPS.Component
{
    public static class ColorHelper
    {
        private static readonly (decimal Ebc, int R, int G, int B)[] Colors =
        [
            (0m, 255, 255, 230),
            (4m, 255, 230, 120),
            (8m, 245, 190, 70),
            (12m, 225, 150, 40),
            (16m, 205, 120, 30),
            (20m, 185, 90, 25),
            (26m, 160, 70, 25),
            (33m, 130, 55, 25),
            (39m, 105, 45, 25),
            (47m, 80, 35, 25),
            (57m, 60, 30, 25),
            (69m, 45, 25, 20),
            (79m, 35, 22, 18),
            (100m, 25, 20, 18)
        ];

        public static string EbcToHex(decimal ebc)
        {
            if (ebc <= 0)
                return "#adb5bd";

            if (ebc >= Colors[^1].Ebc)
                return ToHex(Colors[^1]);

            for (var i = 0; i < Colors.Length - 1; i++)
            {
                var from = Colors[i];
                var to = Colors[i + 1];

                if (ebc < from.Ebc || ebc > to.Ebc)
                    continue;

                var factor = (ebc - from.Ebc) / (to.Ebc - from.Ebc);

                var r = Interpolate(from.R, to.R, factor);
                var g = Interpolate(from.G, to.G, factor);
                var b = Interpolate(from.B, to.B, factor);

                return $"#{r:X2}{g:X2}{b:X2}";
            }

            return "#adb5bd";
        }

        private static int Interpolate(int from, int to, decimal factor)
        {
            return (int)Math.Round(from + (to - from) * factor);
        }

        private static string ToHex((decimal Ebc, int R, int G, int B) color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
