using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Arcoro.Common.Helper;
using ArcoroSamples.hh2;
using ArcoroSamples.sage;
using Newtonsoft.Json;

namespace ArcoroSamples.sage
{
    public class Steps
    {
        public Steps()
        {
            
        }
        public async Task TestAuthentication(Config config, string overrideUser = null, string overridePw = null)
        {
            var client = new HttpClient { BaseAddress = new Uri($"https://{config.HH2Subdomain}.hh2.com/") };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var responseBody = "";
            var token = AuthenticationResult.NoResponse;

            var request = new
            {
                ApiKey = Util.GetPassword("hh2apikey"),
                ApiSecret = Util.GetPassword("hh2apisecret"),
                Username = overrideUser ?? Util.GetPassword("hh2username"),
                Password = overridePw ?? Util.GetPassword(config.HH2Subdomain)
            };

            var response = client.PostAsJsonAsync(config.BaseURI + "Api/Security/V3/Session.svc/authenticate", request);
            if (response.Result.IsSuccessStatusCode)
            {
                responseBody = await response.Result.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Connection Failed!");
            }

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                token = JsonConvert.DeserializeObject<Authentication>(responseBody).Result;
            }

            if (token != AuthenticationResult.Validated)
            {
                throw new Exception($"Authentication Failed: {token}");
            }
	
            token.Dump();
        }
        
        public async Task GetCertifiedClassesExample(Config config)
        {
            //authenticate
            var client = new HttpClient {BaseAddress = new Uri($"https://{config.HH2Subdomain}.hh2.com/")};
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var responseBody = "";
            var token = AuthenticationResult.NoResponse;

            var request = new
            {
                ApiKey = Util.GetPassword("hh2apikey"),
                ApiSecret = Util.GetPassword("hh2apisecret"),
                Username = Util.GetPassword("hh2username"),
                Password = Util.GetPassword(config.HH2Subdomain)
            };

            var response = client.PostAsJsonAsync(config.BaseURI + "Api/Security/V3/Session.svc/authenticate", request);
            if (response.Result.IsSuccessStatusCode)
            {
                responseBody = await response.Result.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Connection Failed!");
            }

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                token = JsonConvert.DeserializeObject<Authentication>(responseBody).Result;
            }
            
            if (token != AuthenticationResult.Validated)
            {
                throw new Exception($"Authentication Failed: {token}");
            }

            // get certified classes
            var results = new List<CertifiedClass>();
            var jsonResponse = new HttpResponseMessage();
            var verResults = new List<CertifiedClass>();
            int recordCount = 999999;
            long version = 0;

            var urlFull = "RemotePayroll/Api/Core/CertifiedClass.svc/certifiedclasses?version=";

            while (recordCount >= config.VersionBlockMax)
            {
                try
                {
                    jsonResponse = await client.GetAsync(urlFull + version).ConfigureAwait(false);

                    if (jsonResponse.IsSuccessStatusCode)
                    {
                        var content = await jsonResponse.Content.ReadAsStringAsync();

                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            if (content.StartsWith("[") || content.StartsWith("{"))
                            {
                                verResults = JsonConvert.DeserializeObject<List<CertifiedClass>>(content);
                            }
                            else
                                return;
                        }
                        else return;
                    }
                    else
                    {
                        var error = jsonResponse.Content.ReadAsStringAsync().Result;
                        Util.RawHtml(error).Dump();
                        return;
                    }

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            results.Dump();
        }

        public async Task GetCoreSetupData(Config config)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HH2API api = await HH2API.Create(config);

            var task1 = api.ConvertToLiteAndWriteToFile(GET.GetAllCertifiedClasses);
            var task2 = api.ConvertToLiteAndWriteToFile(GET.GetAllDepartments);
            var task3 = api.ConvertToLiteAndWriteToFile(GET.GetAllPayGroups);
            var task4 = api.ConvertToLiteAndWriteToFile(GET.GetAllPayTypes);
            var task5 = api.ConvertToLiteAndWriteToFile(GET.GetAllUnions);
            var task6 = api.ConvertToLiteAndWriteToFile(GET.GetAllUnionClasses);
            var task7 = api.ConvertToLiteAndWriteToFile(GET.GetAllUnionLocals);
            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);

            sw.Stop();
            ("GetCoreSetupData TTC: " + sw.Elapsed).Dump();
        }

        public async Task GetEEUsedSetupData(Config config)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HH2API api = await HH2API.Create(config);

            var allEmployees = ((List<Employee>) await api.GetAllEmployees());

            var task1 = api.GetAllUsedWorkersCompCodes(allEmployees);
            var task2 = api.GetAllUsedFrequencies(allEmployees);
            var task3 = api.GetAllUsedWorkStates(allEmployees);

            await Task.WhenAll(task1, task2, task3);

            sw.Stop();
            ("GetEEUsedSetupData TTC: " + sw.Elapsed).Dump();
        }

        public async Task GetEmployeesSetupData(Config config)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HH2API api = await HH2API.Create(config);

            var allEEs = ((List<Employee>) await api.GetAllEmployees());
            var eePayTaxes = (List<EEPayTax>) await api.GetAllEmployeePayTaxes();

            await api.GetAllEmployeeDemographics(allEEs);

            await api.GetAllEmployeeFederalTaxes(allEEs, eePayTaxes);

            await api.GetAllEmployeeStateTaxes(allEEs, eePayTaxes);

            await api.GetAllEmployeeCompensation(allEEs);

            await api.GetAllEmployeeDirectDeposits(allEEs);

            sw.Stop();
            ("GetEmployeesSetupData TTC: " + sw.Elapsed).Dump();
        }

        public async Task SubscribeToCompanyNotifications(Config config)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HH2API api = await HH2API.Create(config);

            await api.SubscribeToNotification(SubscriptionMode.PerType, NotificationTypeEntities.CertifiedClass);
            await api.SubscribeToNotification(SubscriptionMode.PerType, NotificationTypeEntities.Department);
            await api.SubscribeToNotification(SubscriptionMode.PerType, NotificationTypeEntities.PayType);
            await api.SubscribeToNotification(SubscriptionMode.PerType, NotificationTypeEntities.Union);
            await api.SubscribeToNotification(SubscriptionMode.PerType, NotificationTypeEntities.UnionClass);
            await api.SubscribeToNotification(SubscriptionMode.PerType, NotificationTypeEntities.UnionLocal);
                                                                
            sw.Stop();
            ("SubscribeToCompanyNotifications TTC: " + sw.Elapsed).Dump();
        }

        public async Task GetNotificationsSubscribedTo(Config config)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HH2API api = await HH2API.Create(config);

            await api.GetNotificationsList();

            sw.Stop();
            ("GetNotificationSubscribedTo TTC: " + sw.Elapsed).Dump();
        }

        public async Task GetChangeNotifications(Config config, long lastVersion = 0)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HH2API api = await HH2API.Create(config);

            await api.GetLatestChanges(lastVersion);

            sw.Stop();
            ("GetChangeNotifications TTC: " + sw.Elapsed).Dump();
        }

        public async Task GetCoreSetupDataUpdates(Config config)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HH2API api = await HH2API.Create(config);
            var tasks = new List<Task>();

            // In place of having stored values in a database:
            var lastVersions = new Dictionary<string, long>
            {
                {"ChangeEvent", 0},
                {NotificationTypeEntities.CertifiedClass, 18291},
                {NotificationTypeEntities.Department, 18305},
                {NotificationTypeEntities.PayType, 18405},
                {NotificationTypeEntities.Union, 18413},
                {NotificationTypeEntities.UnionClass, 19504},
                {NotificationTypeEntities.UnionLocal, 19465}
            };

            var typeToMethodMapping = new Dictionary<string, GET>
            {
                {NotificationTypeEntities.CertifiedClass, GET.GetAllCertifiedClasses},
                {NotificationTypeEntities.Department, GET.GetAllDepartments},
                {NotificationTypeEntities.PayType, GET.GetAllEmployeePay},
                {NotificationTypeEntities.Union, GET.GetAllUnions},
                {NotificationTypeEntities.UnionClass, GET.GetAllUnionClasses},
                {NotificationTypeEntities.UnionLocal, GET.GetAllUnionLocals}
            };

            var changes = (List<ChangeEvent>) await api.GetAllChangeEvents(lastVersions["ChangeEvent"]);

            if (changes.Count() > 0)
            {
                await Task.WhenAll(changes.Select(i => api.ConvertToLiteAndWriteToFile(typeToMethodMapping[i.TypeId.ToUpper()], lastVersions[i.TypeId.ToUpper()].ToString())));
            }
            else
            {
                "No updates found!".Dump();
            }


            sw.Stop();
            ("GetCoreSetupDataUpdates TTC: " + sw.Elapsed).Dump();
        }
    }

}
