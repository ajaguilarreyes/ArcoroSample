using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArcoroSamples
{
    class Program
    {
        public static IConfiguration configuration;
        
        static void Main(string[] args)
        {
            try
            {
                MainAsync(args).Wait();
            }
            catch
            {
                throw;
            }
        }

        static async Task MainAsync(string[] args)
        {
            var serviceCollection = ConfigureServices(new ServiceCollection());
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            
            try
            {
                await serviceProvider.GetService<IApp>().Start();
            }
            catch
            {
                throw;
            }
        }

        private static ServiceCollection ConfigureServices(ServiceCollection serviceCollection)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var apiKey = Environment.GetEnvironmentVariable("SAGE_APIKEY");
            var apiSecret = Environment.GetEnvironmentVariable("SAGE_SECRET");
            
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddScoped<IApp, App>();

            return serviceCollection;
        }
    }
}



