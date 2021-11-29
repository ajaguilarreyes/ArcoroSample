using System.Threading.Tasks;
using ArcoroSamples.sage;
using Microsoft.Extensions.Configuration;

namespace ArcoroSamples
{
    public interface IApp
    {
        Task Start();
    }

    public class App : IApp
    {
        private readonly IConfiguration _configuration;
        private readonly Config _config;

        public App(IConfiguration configuration)
        {
            _configuration = configuration;
            _config = new Config(configuration);
        }

        public async Task Start()
        {
            #region Parameters

            var config = new Config(
                hh2Subdomain: @"arcoro_test_300", //This is specific per client/company. Arcoro_test_300 is our test instance
                exportCSVPath: @"C:\Users\MikeG\OneDrive\Documents\Work\Arcoro\Temp\", //File path where any CSVs that get created should get saved to.
                versionBlockMax: 1000, // Max number of records returned by the API per call and 1000 is the hard-coded set value by HH2. Do not change. 
                maxLoopCall: 15, // Arbitrary number I created to prevent a loop from going to long, more troubleshooting. Probably NOT something you want to implement.
                showJson: JsonMode.Off, //Off = do not show any Json on screen. AllRecords = show the full Json. FirstRecord = displays the first record in the Json only. Troubleshooting purposes only.
                showEndpoint: true, //Shows endpoint being called on screen. Can set to false to hide.
                writeToScreen: true //If set to true, will display on the screen instead of writing to file. If set to false, will write to the folder designated in exportCSVPath.
            );

            #endregion

            var steps = new Steps();

            await steps.TestAuthentication(_config); //Tests authentication and prints results to screen.
            //await steps.GetCertifiedClassesExample(config);	//Simple full example of authenticating and then returning all the certified classes for API.
            //await steps.GetCoreSetupData(config);	//Example of all of the company level (step 1) data we are trying to pull. The method here is a bit complicated since I'm using delegates for this one.
            //await steps.GetEEUsedSetupData(config);	//Example of getting the "in use" setup data. For example, we don't want to set up all 50 states in Core, we only want which states are being used.
            //await steps.GetEmployeesSetupData(config); //Example of pulling data for all of the employee level data we want to import into Core
            //await steps.SubscribeToCompanyNotifications(config);	//In order to be able to get only changes that have been made, must subscribe for each data category (per company).
            //await steps.GetNotificationsSubscribedTo(config);	//Displays the list of subscriptions (per company) currently registered.
            //await steps.GetChangeNotifications(config, 0); //lastVersion = 92090 | This is how to retreive changes. Send in the last "version" checked for, and it will return what categories have been updated since.
            //await steps.GetCoreSetupDataUpdates(config);	//This example checks for changes, and then gets those changes per category.
        }
    }
}