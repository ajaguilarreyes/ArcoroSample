using Arcoro.Common.Model.Enum;

namespace Arcoro.Common.Configuration
{
    public class AppSettings
    {
        public string HH2Subdomain { get; }
        public string BaseURI { get; set; }
        public string ExportCSVPath { get; }
        public JsonMode ShowJson { get; }
        public bool ShowEndpoint { get; }
        public bool WriteToScreen { get; }
        public int VersionBlockMax { get; }
        public int MaxLoopCall { get; }
    }
}
