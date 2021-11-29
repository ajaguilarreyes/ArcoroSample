namespace Arcoro.Common.Model.Employee
{
    public class EEDeduction
    {
        public double Amount { get; set; }
        public int CalculationMethod { get; set; }
        public int CalculationOrder { get; set; }
        public int? DeductionType { get; set; }
        public string EmployeeId { get; set; }
        public string Formula { get; set; }
        public int Frequency { get; set; }
        public string Id { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAutomatic { get; set; }
        public bool IsPrenotification { get; set; }
        public double LimitAmount { get; set; }
        public int LimitPeriod { get; set; }
        public string PayDeductionId { get; set; }
        public string PrenotificationDate { get; set; }
        public double TransferDeductionAmount { get; set; }
        public long Version { get; set; }
    }
}