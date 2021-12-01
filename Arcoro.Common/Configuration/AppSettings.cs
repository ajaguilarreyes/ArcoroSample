using System.Net.Http;
using Arcoro.Common.Model.Enum;

namespace Arcoro.Common.Configuration
{
    public class AppSettings
    {
        public AppSettings()
        {
            
        }

        public AppSettings(string hh2Subdomain, string exportCSVPath, JsonMode showJson, bool showEndpoint, bool writeToScreen, int versionBlockMax, int maxLoopCall)
        {
            _hh2Subdomain = hh2Subdomain;
            _exportCsvPath = exportCSVPath;
            _showJson = showJson;
            _showEndpoint = showEndpoint;
            _writeToScreen = writeToScreen;
            _versionBlockMax = versionBlockMax;
            _maxLoopCall = maxLoopCall;
            _baseURI = $"https://{hh2Subdomain}.hh2.com/";
        }

        public string HH2Subdomain => _hh2Subdomain;
        private readonly string _hh2Subdomain;

        public string BaseURI { get => _baseURI; set { _baseURI = value; } }
        private string _baseURI;

        public string ExportCSVPath => _exportCsvPath;
        private readonly string _exportCsvPath;

        public JsonMode ShowJson => _showJson;
        private readonly JsonMode _showJson;

        public bool ShowEndpoint => _showEndpoint;
        private readonly bool _showEndpoint;

        public bool WriteToScreen => _writeToScreen;
        private readonly bool _writeToScreen;

        public int VersionBlockMax => _versionBlockMax;
        private readonly int _versionBlockMax;

        public int MaxLoopCall => _maxLoopCall;
        private readonly int _maxLoopCall;

        public HttpClient Client { get; set; }
    }
}
