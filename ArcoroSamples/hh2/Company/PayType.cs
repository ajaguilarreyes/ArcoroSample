namespace ArcoroSamples.hh2
{
    public class PayType
    {
        public string Abbreviation { get; set; }
        public string Code { get; set; }
        public int CodingEnforcementMode { get; set; }
        public string DefaultColor { get; set; }
        public string DefaultColorId { get; set; }
        public string Description { get; set; }
        public int GlobalOrder { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public bool IsCodable { get; set; }
        public int Precision { get; set; }
        public bool ShouldDisplayHourlyRate { get; set; }
        public bool ShouldDisplayOnYtdAddendum { get; set; }
        public bool ShouldDisplayUnits { get; set; }
        public bool ShouldImportToEquipmentRevenue { get; set; }
        public bool ShouldIncludeInTotals { get; set; }
        public int Type { get; set; }
        public int Version { get; set; }
    }
}