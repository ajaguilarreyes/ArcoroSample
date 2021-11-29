using System;

namespace Arcoro.Service
{
    internal class Program
    {
        public static IConfiguration configuration;

        private static void Main(string[] args)
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

        private static async Task MainAsync(string[] args)
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
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string apiKey = Environment.GetEnvironmentVariable("SAGE_APIKEY");
            string apiSecret = Environment.GetEnvironmentVariable("SAGE_SECRET");

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
