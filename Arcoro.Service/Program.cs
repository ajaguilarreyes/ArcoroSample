using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcoro.Service
{
    internal class Program
    {
        public static IConfiguration configuration;

        private static void Main(string[] args) => MainAsync(args).Wait();

        private static async Task MainAsync(string[] args)
        {
            var serviceCollection = ConfigureServices(new ServiceCollection());
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider.GetService<IApp>().Start();
        }

        private static ServiceCollection ConfigureServices(ServiceCollection serviceCollection)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

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
