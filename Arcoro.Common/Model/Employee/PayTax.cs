namespace Arcoro.Common.Model.Employee
{
    public class PayTax
    {
        public string Abbreviation { get; set; }
        public string Code { get; set; }
        public string Formula { get; set; }
        public string Id { get; set; }
        public bool IsEmployerTax { get; set; }
        public double LimitAmount { get; set; }
        public string Name { get; set; }
        public int TaxLevel { get; set; }
        public int Version { get; set; }
    }
}