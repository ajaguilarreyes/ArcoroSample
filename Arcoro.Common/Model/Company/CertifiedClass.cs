namespace Arcoro.Common.Model.Company
{
    public class CertifiedClass
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public string Name { get; set; }
    }
}