using Microsoft.Extensions.Configuration;
using System.IO;

namespace ParticipantsCounter.App.Infrastructure
{
    public static class ApplicationSettingsManager
    {
        private static IConfiguration _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

                    _configuration = builder.Build();
                }

                return _configuration;
            }
        }

        public static string Token => Configuration["token"];

        public static string ProxyAddress => Configuration["proxySettings:address"];

        public static string ProxyPort => Configuration["proxySettings:port"];        
    }
}