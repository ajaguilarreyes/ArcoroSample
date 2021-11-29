using System.Text.Json.Serialization;

namespace Arcoro.Common.Model.Employee
{
    public class DEMOGRAPHIC
    {
        [JsonPropertyName("ID")]
        public string EEId { get; set; }
        [JsonPropertyName("Code")]
        public string EECode { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressPostalCode { get; set; }
        public string CellPhoneNumber { get; set; }
        public string MainPhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public int Gender { get; set; }
        public string EmailAddress { get; set; }
        public string DefaultDepartmentId { get; set; }
        public string DefaultDepartment { get; set; }
        public string DefaultPayTypeId { get; set; }
        public string DefaultPayType { get; set; }
        public int SkillLevel { get; set; }
        public string RehireDate { get; set; }
        public string HireDate { get; set; }
        public string TerminationDate { get; set; }
        public int Ethnicity { get; set; }
        public string Occupation { get; set; }
        public string WorkersCompCode { get; set; }
        public string DefaultCertifiedClassId { get; set; }
        public string DefaultCertifiedClass { get; set; }
        public string DefaultUnionClassId { get; set; }
        public string DefaultUnionClass { get; set; }
        public string DefaultUnionId { get; set; }
        public string DefaultUnion { get; set; }
        public string DefaultUnionLocalId { get; set; }
        public string DefaultUnionLocal { get; set; }
        public string WorkState { get; set; }
        public string PayGroupId { get; set; }
        public string PayGroup { get; set; }
        public int? PayFrequency { get; set; }
        public bool TaxExemptionCount { get; set; }
        public string Title { get; set; }
        public bool ShouldUseW4Amounts { get; set; }
        public decimal W4DeductionsAmount { get; set; }
        public decimal W4DependentsAmount { get; set; }
        public bool W4HasSpecifiedTwoJobs { get; set; }
        public decimal W4OtherIncomeAmount { get; set; }
        public int FilingStatus { get; set; }
    }
}