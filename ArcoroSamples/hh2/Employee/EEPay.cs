namespace ArcoroSamples.hh2
{
    public class EEPay
    {
        public double Amount { get; set; }
        public int CalculationMethod { get; set; }
        public long CalculationOrder { get; set; }
        public string EffectiveOnUtc { get; set; }
        public string EmployeeId { get; set; }
        public string ExpiresOnUtc { get; set; }
        public string Formula { get; set; }
        public int Frequency { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAutomatic { get; set; }
        public double LimitAmount { get; set; }
        public int LimitPeriod { get; set; }
        public string PayTypeId { get; set; }
        public long Version { get; set; }
    }
}