using System.Net.Http;
using ArcoroSamples.common;
using Microsoft.Extensions.Configuration;

namespace ArcoroSamples.sage
{
    public class Config
    {
        private readonly IConfiguration _configuration;
        public string HH2Subdomain { get; }
        public string BaseURI { get; set; }
        public string ExportCSVPath { get; }
        public JsonMode ShowJson { get; }
        public bool ShowEndpoint { get; }
        public bool WriteToScreen { get; }
        public int VersionBlockMax { get; }
        public int MaxLoopCall { get; }
        public HttpClient Client { get; set; }
        
        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
            
            HH2Subdomain = configuration["ArcoroSecrets.hh2Subdomain"];
            BaseURI = configuration["ArcoroSecrets.baseURI"];
            ExportCSVPath = configuration["ArcoroSecrets.exportCSVPath"];
            ShowJson = configuration["ArcoroSecrets.showJson"].ToEnum<JsonMode>();
            
            var showEndpoint = false;
            bool.TryParse(configuration["ArcoroSecrets.showEndpoint"], out showEndpoint);
            ShowEndpoint = showEndpoint;

            var writeToScreen = false;
            bool.TryParse(configuration["ArcoroSecrets.writeToScreen"], out writeToScreen);
            WriteToScreen = writeToScreen;

            var versionBlockMax = 0;
            int.TryParse(configuration["ArcoroSecrets.versionBlockMax"], out versionBlockMax);
            VersionBlockMax = versionBlockMax;
            
            var maxLoopCall = 0;
            int.TryParse(configuration["ArcoroSecrets.maxLoopCall"], out maxLoopCall);
            MaxLoopCall = maxLoopCall;
        }
        
        public Config(string hh2Subdomain, string exportCSVPath, JsonMode showJson, bool showEndpoint, bool writeToScreen, int versionBlockMax, int maxLoopCall)
        {
            HH2Subdomain = hh2Subdomain;
            ExportCSVPath = exportCSVPath;
            ShowJson = showJson;
            ShowEndpoint = showEndpoint;
            WriteToScreen = writeToScreen;
            VersionBlockMax = versionBlockMax;
            MaxLoopCall = maxLoopCall;
        }
        public Config(string hh2Subdomain, string baseURI, string exportCSVPath, JsonMode showJson, bool showEndpoint, bool writeToScreen, int versionBlockMax, int maxLoopCall)
        {
            HH2Subdomain = hh2Subdomain;
            BaseURI = baseURI;
            ExportCSVPath = exportCSVPath;
            ShowJson = showJson;
            ShowEndpoint = showEndpoint;
            WriteToScreen = writeToScreen;
            VersionBlockMax = versionBlockMax;
            MaxLoopCall = maxLoopCall;
        }
    }
}