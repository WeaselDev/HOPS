namespace HOPS.Pages.BrewBatchPages
{
    public class BrewAddition
    {
        public string Name { get; set; } = string.Empty; 
        public decimal AlphaAcid { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int Duration { get; set; }
        public bool IsVirtual { get; set; }
        public int TimerTime { get; set; }
    }


    public class IrishMossAddition : BrewAddition
    {
        public IrishMossAddition()
        {
            Name = "Irish Moss";
            Amount = 5.0m;
            Unit = "g";
            Duration = 15;
            IsVirtual = true;
        }
    }

    public class WhirlflocAddition : BrewAddition
    {
        public WhirlflocAddition()
        {
            Name = "Whirlfloc";
            Amount = 0.5m;
            Unit = "tablet";
            Duration = 15;
            IsVirtual = true;
        }
    }
}
