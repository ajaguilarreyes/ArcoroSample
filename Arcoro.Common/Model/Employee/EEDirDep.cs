namespace Arcoro.Common.Model.Employee
{
    public class EEDirDep
    {
        public double Amount { get; set; }
        public int CalculationMethod { get; set; }
        public int? DeductionType { get; set; }
        public string EmployeeId { get; set; }
        public int Frequency { get; set; }
        public bool IsAutomatic { get; set; }
        public bool IsPrenotification { get; set; }
        public double LimitAmount { get; set; }
        public string PayDeductionId { get; set; }
        public string BankAccount { get; set; }
        public int? BankAccountType { get; set; }
        public string BankIdentifier { get; set; }
    }
}