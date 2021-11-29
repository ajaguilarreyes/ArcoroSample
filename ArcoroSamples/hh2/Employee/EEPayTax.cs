namespace ArcoroSamples.hh2
{
    public class EEPayTax
    {
        public double? AdjustmentAmount { get; set; }
        public int AdjustmentMethod { get; set; }
        public string EmployeeId { get; set; }
        public string Id { get; set; }
        public bool IsArchived { get; set; }
        public string PayTaxId { get; set; }
        public int TaxLevel { get; set; }
        public long Version { get; set; }
    }
}