namespace ArcoroSamples.hh2
{
    public class EEStateTax
    {
        public string EmployeeId { get; set; }
        public string FilingStatusCode { get; set; }
        public string Id { get; set; }
        public bool IsArchived { get; set; }
        public string MiscellaneousTaxCode1 { get; set; }
        public string MiscellaneousTaxCode2 { get; set; }
        public string MiscellaneousTaxCode3 { get; set; }
        public bool ShouldUseFederalFilingInformation { get; set; }
        public string State { get; set; }
        public long TaxExemptionCount { get; set; }
        public long Version { get; set; }
    }
}