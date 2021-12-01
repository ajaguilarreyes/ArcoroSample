using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Arcoro.Common.Configuration;
using Arcoro.Common.Helper;
using Arcoro.Common.Model.Company;
using Arcoro.Common.Model.Employee;
using Arcoro.Common.Model.Enum;
using Arcoro.Common.Model.Notification;
using Arcoro.Common.Model.Setup;
// using ArcoroSamples.hh2;
using Newtonsoft.Json;

namespace ArcoroSamples.sage
{
    public class HH2API
    {
        private AppSettings _config { get; }

        public async Task GetAllEmployeeDemographics(List<Employee> eeList)
        {
            var eeDemographics = await BuildEmployeeDemographicsByEEIdList(eeList);

            writeToCSV(eeDemographics, "GetAllEmployeeDemographics");
        }

        public async Task GetAllEmployeeFederalTaxes(List<Employee> eeList, List<EEPayTax> eePayTaxes)
        {
            IEnumerable<object> federalTaxes = null;

            await Task.Run(() =>
            {
                federalTaxes = from pt in eePayTaxes
                    join ee in eeList on pt.EmployeeId equals ee.Id
                    select new
                    {
                        EEId = ee.Id, EECode = ee.Code, ee.FilingStatus, ee.MiscellaneousTaxCode, ee.ShouldUseW4Amounts, ee.TaxExemptionCount, ee.W4DeductionsAmount, ee.W4DependentsAmount,
                        ee.W4ExtraWithholdingsAmount, ee.W4HasSpecifiedTwoJobs, ee.W4OtherIncomeAmount, pt.AdjustmentMethod, pt.AdjustmentAmount
                    };
            });

            writeToCSV(federalTaxes, "GetAllEmployeeFederalTaxes");
        }

        public async Task GetAllEmployeeStateTaxes(List<Employee> eeList, List<EEPayTax> eePayTaxes)
        {
            IEnumerable<object> stateTaxes = null;

            var allEEStateTaxes = (List<EEStateTax>) await GetAllEmployeeStateTaxes();
            await Task.Run(() =>
            {
                stateTaxes = from st in allEEStateTaxes
                    join pt in eePayTaxes on st.EmployeeId equals pt.EmployeeId
                    join ee in eeList on st.EmployeeId equals ee.Id
                    select new
                    {
                        EEId = ee.Id, EECode = ee.Code, st.State, FilingStatus = st.FilingStatusCode, MiscTaxCode = st.MiscellaneousTaxCode1, MiscTaxCode2 = st.MiscellaneousTaxCode2,
                        MiscTaxCode3 = st.MiscellaneousTaxCode3, UseFederal = st.ShouldUseFederalFilingInformation, Exemptions = st.TaxExemptionCount, pt.AdjustmentMethod, pt.AdjustmentAmount,
                        pt.IsArchived
                    };
            });

            writeToCSV(stateTaxes, "GetAllEmployeeStateTaxes");
        }

        public async Task GetAllEmployeeCompensation(List<Employee> eeList)
        {
            IEnumerable<object> compensation = null;

            var allEEPays = (List<EEPay>) await GetAllEmployeePay();
            var allPays = (List<PayType>) await GetAllPayTypes();
            await Task.Run(() =>
            {
                compensation = from p in allEEPays
                    join ap in allPays on p.PayTypeId equals ap.Id
                    join ee in eeList on p.EmployeeId equals ee.Id
                    select new {EEId = ee.Id, EECode = ee.Code, ap.Abbreviation, ap.Code, ap.Description, p.Amount, p.EffectiveOnUtc, p.ExpiresOnUtc, p.Id, p.PayTypeId};
            });

            writeToCSV(compensation, "GetAllEmployeeCompensation");
        }

        public async Task GetAllEmployeeDirectDeposits(List<Employee> eeList)
        {
            var eeDirectDeposits = await BuildEmployeeDirDepByEEIdList(eeList);

            writeToCSV(eeDirectDeposits, "GetAllEmployeeDirectDeposits");
        }

        public async Task GetAllUsedWorkersCompCodes(List<Employee> eeList)
        {
            IEnumerable<string> usedWCC = null;
            var listWCCodes = new List<object>();

            await Task.Run(() => { usedWCC = eeList.Where(e => e.WorkersCompensationCode != null).GroupBy(g => g.WorkersCompensationCode).Select(s => s.First().WorkersCompensationCode); });

            foreach (var code in usedWCC)
            {
                listWCCodes.Add(new {WCCode = code});
            }

            writeToCSV(listWCCodes, "GetAllUsedWorkersCompCodes");
        }

        public async Task GetAllUsedFrequencies(List<Employee> eeList)
        {
            IEnumerable<int> usedFrequencies = null;
            var listFrequencies = new List<object>();

            await Task.Run(() => { usedFrequencies = eeList.Where(e => e.PayFrequency != 0).GroupBy(g => g.PayFrequency).Select(s => s.First().PayFrequency); });

            foreach (var code in usedFrequencies)
            {
                listFrequencies.Add(new {Frequency = ((PayFrequency) code).ToString()});
            }

            writeToCSV(listFrequencies, "GetAllUsedFrequencies");
        }

        public async Task GetAllUsedWorkStates(List<Employee> eeList)
        {
            IEnumerable<string> usedWorkStates = null;
            var listWorkStates = new List<object>();

            await Task.Run(() => { usedWorkStates = eeList.Where(e => e.WorkState != null).GroupBy(g => g.WorkState).Select(s => s.First().WorkState); });

            foreach (var state in usedWorkStates)
            {
                listWorkStates.Add(new {State = state});
            }

            writeToCSV(listWorkStates, "GetAllUsedWorkStates");
        }

        public async Task GetNotificationsList()
        {
            IEnumerable<object> notifications = null;
            var listNotifications = (List<Registration>) await GetNotificationRegistrations();

            var notificationType = GetStaticConstantDictionary(typeof(NotificationTypeEntities));

            await Task.Run(() => { notifications = listNotifications.Select(n => new {n.CreatedOnUtc, Mode = (SubscriptionMode) n.Mode, n.TypeId, TypeName = notificationType[n.TypeId.ToUpper()]}); });

            writeToCSV(notifications, "GetNotificationsList");
        }

        public async Task GetLatestChanges(long Version)
        {
            IEnumerable<object> changes = null;
            var listChangeEvents = (List<ChangeEvent>) await GetAllChangeEvents();

            var notificationType = GetStaticConstantDictionary(typeof(NotificationTypeEntities));

            await Task.Run(() => { changes = listChangeEvents.Select(n => new {n.LastUpdatedOnUtc, n.RootEntityId, n.TypeId, TypeName = notificationType[n.TypeId.ToUpper()], n.Version}); });

            writeToCSV(changes, "GetLatestChanges");
        }

        private async Task<List<Employee>> DecodeDemographics(List<Employee> eeList)
        {
            var task1 = GetAllCertifiedClasses();
            var task2 = GetAllPayGroups();
            var task3 = GetAllPayTypes();
            var task4 = GetAllDepartments();
            var task5 = GetAllUnions();
            var task6 = GetAllUnionClasses();
            var task7 = GetAllUnionLocals();

            await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);

            var certClasses = (List<CertifiedClass>) task1.Result;
            var payGroups = (List<PayGroup>) task2.Result;
            var payTypes = (List<PayType>) task3.Result;
            var departments = (List<Department>) task4.Result;
            var unions = (List<Union>) task5.Result;
            var unionClasses = (List<UnionClass>) task6.Result;
            var unionLocals = (List<UnionLocal>) task7.Result;

            var joinedDemo = from f in eeList
                join c in certClasses on f?.DefaultCertifiedClassId equals c.Id into cc
                join p in payGroups on f?.PayGroupId equals p.Id into pg
                join pay in payTypes on f?.DefaultPayTypeId equals pay.Id into pt
                join d in departments on f?.DefaultDepartmentId equals d.Id into dt
                join u in unions on f?.DefaultUnionId equals u.Id into ut
                join uc in unionClasses on f?.DefaultUnionClassId equals uc.Id into uct
                join ul in unionLocals on f?.DefaultUnionLocalId equals ul.Id into ult
                from c in cc.DefaultIfEmpty()
                from p in pg.DefaultIfEmpty()
                from pay in pt.DefaultIfEmpty()
                from d in dt.DefaultIfEmpty()
                from u in ut.DefaultIfEmpty()
                from uc in uct.DefaultIfEmpty()
                from ul in ult.DefaultIfEmpty()
                select new Employee
                {
                    Id = f.Id,
                    Code = f.Code,
                    FirstName = f.FirstName,
                    MiddleName = f.MiddleName,
                    LastName = f.LastName,
                    AddressCity = f.AddressCity,
                    AddressState = f.AddressState,
                    AddressPostalCode = f.AddressPostalCode,
                    CellPhoneNumber = f.CellPhoneNumber,
                    MainPhoneNumber = f.MainPhoneNumber,
                    Gender = f.Gender,
                    EmailAddress = f.EmailAddress,
                    DefaultDepartmentId = f.DefaultDepartmentId,
                    DefaultDepartment = (f.DefaultDepartmentId == null) ? null : (String.IsNullOrEmpty(d?.Id) ? "NOT FOUND" : $"({d?.Code}) {d?.Name}"),
                    DefaultUnionClassId = f.DefaultUnionClassId,
                    DefaultUnionClass = (f.DefaultUnionClassId == null) ? null : (String.IsNullOrEmpty(uc?.Id) ? "NOT FOUND" : $"({uc?.Code}) {uc?.Name}"),
                    DefaultUnionId = f.DefaultUnionId,
                    DefaultUnion = (f.DefaultUnionId == null) ? null : (String.IsNullOrEmpty(u?.Id) ? "NOT FOUND" : $"({u?.Code}) {u?.Name}"),
                    DefaultUnionLocalId = f.DefaultUnionLocalId,
                    DefaultUnionLocal = (f.DefaultUnionLocalId == null) ? null : (String.IsNullOrEmpty(ul?.Id) ? "NOT FOUND" : $"({ul?.Code}) {ul?.Name}"),
                    RehireDate = f.RehireDate,
                    HireDate = f.HireDate,
                    TerminationDate = f.TerminationDate,
                    Ethnicity = f.Ethnicity,
                    Occupation = f.Occupation,
                    DefaultCertifiedClassId = f.DefaultCertifiedClassId,
                    DefaultCertifiedClass = (f.DefaultCertifiedClassId == null) ? null : (String.IsNullOrEmpty(c?.Id) ? "NOT FOUND" : $"({c?.Code}) {c?.Name}"),
                    WorkState = f.WorkState,
                    PayGroupId = f.PayGroupId,
                    PayGroup = (f.PayGroupId == null) ? null : (String.IsNullOrEmpty(p?.Id) ? "NOT FOUND" : $"({p?.Code}) {p?.Name}"),
                    DefaultPayTypeId = f.DefaultPayTypeId,
                    DefaultPayType = (f.DefaultPayTypeId == null) ? null : (String.IsNullOrEmpty(pay?.Id) ? "NOT FOUND" : $"({pay?.Code}) {pay?.Description}"),
                    Title = f.Title,
                    TaxExemptionCount = f.TaxExemptionCount,
                    PayFrequency = f.PayFrequency,
                    WorkersCompensationCode = f.WorkersCompensationCode,
                    ShouldUseW4Amounts = f.ShouldUseW4Amounts,
                    SkillLevel = f.SkillLevel,
                    W4DeductionsAmount = f.W4DeductionsAmount,
                    W4DependentsAmount = f.W4DependentsAmount,
                    W4HasSpecifiedTwoJobs = f.W4HasSpecifiedTwoJobs,
                    W4OtherIncomeAmount = f.W4OtherIncomeAmount,
                    FilingStatus = f.FilingStatus
                };

            return joinedDemo.ToList();
        }

        public async Task<bool> ConvertToLiteAndWriteToFile(GET method, string arg1 = null, string arg2 = null, string arg3 = null, string arg4 = null)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(method.ToString());

            var tArgs = new List<Type>();
            foreach (var param in theMethod.GetParameters())
                tArgs.Add(param.ParameterType);
            tArgs.Add(theMethod.ReturnType);
            var delDecltype = Expression.GetDelegateType(tArgs.ToArray());

            Delegate del = Delegate.CreateDelegate(delDecltype, this, theMethod);

            object results = null;

            int argumentCount = tArgs.Count() - 1;

            if (argumentCount == 1 && (int.TryParse(arg1, out int version)))
            {
                results = await HH2API.invokeMethod(del, version);
            }
            else
            {
                switch (argumentCount)
                {
                    case 1:
                        results = await HH2API.invokeMethod(del, arg1);
                        break;
                    case 2:
                        results = await HH2API.invokeMethod(del, arg1, arg2);
                        break;
                    case 3:
                        results = await HH2API.invokeMethod(del, arg1, arg2, arg3);
                        break;
                    case 4:
                        results = await HH2API.invokeMethod(del, arg1, arg2, arg3, arg4);
                        break;
                    default:
                        results = await HH2API.invokeMethod(del);
                        break;
                }
            }

            string fullType = results.GetType().ToString();
            int startIndex = fullType.IndexOf('[') + 1;

            string liteClassName = fullType.Substring(startIndex, fullType.IndexOf(']') - startIndex) + "Lite";

            Type elementType = Type.GetType(liteClassName);
            Type listType = typeof(List<>).MakeGenericType(new Type[] {elementType});

            object list = Activator.CreateInstance(listType);

            var resultsLite = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(results), list.GetType());

            writeToCSV((IEnumerable) resultsLite, theMethod.Name);

            return true;
        }

        public async Task<List<DEMOGRAPHIC>> BuildEmployeeDemographicsByEEIdList(List<Employee> eeList)
        {
            var eePII = new List<EEPII>();

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            if (eeList != null)
            {
                await ParallelHelper.AsyncParallelForEach(
                    eeList, async ee => eePII.Add(await UpdateEEPII(ee)), 16, TaskScheduler.FromCurrentSynchronizationContext());
            }

            var decodedEEs = await DecodeDemographics(eeList);

            var demographics = (List<DEMOGRAPHIC>) JsonConvert.DeserializeObject(JsonConvert.SerializeObject(decodedEEs), typeof(List<DEMOGRAPHIC>));

            foreach (var emp in eePII)
            {
                var curEE = demographics.FirstOrDefault(ee => emp.EEId.Equals(ee.EEId));
                if (curEE != null)
                {
                    curEE.Address1 = emp.Address1;
                    curEE.Address2 = emp.Address2;
                    curEE.SocialSecurityNumber = emp.SSN;
                    curEE.BirthDate = emp.BirthDate;
                }
            }

            return demographics;
        }

        public async Task<IEnumerable<object>> BuildEmployeeDirDepByEEIdList(List<Employee> eeList)
        {
            var eeDeds = new List<EEDeduction>();

            if (eeList != null)
            {
                eeDeds = (List<EEDeduction>) await GetEEDedsByEEIdList(eeList);
            }

            var eeDirDeps = new List<EEDirDep>();
            var eeAccounts = new List<DedBankAccount>();
            var allPayDeds = ((List<Deduction>) await GetAllDeductions()).Where(pd => pd.Type != 1);
            var filteredDeds = eeDeds.Where(d => allPayDeds.Any(pd => pd.Id == d.PayDeductionId));

            foreach (var ded in filteredDeds)
            {
                var ba = ((List<DedBankAccount>) await GetDeductionBankAccount(ded.Id)).FirstOrDefault();
                var bi = ((List<DedBankIdentifier>) await GetDeductionBankIdentifier(ded.Id)).FirstOrDefault();
                if (ba.BankAccount != null)
                {
                    eeDirDeps.Add(new EEDirDep
                    {
                        EmployeeId = ded.EmployeeId,
                        Amount = ded.Amount,
                        BankAccount = ba.BankAccount,
                        BankAccountType = ba.BankAccountType,
                        BankIdentifier = bi.BankIdentifier,
                        CalculationMethod = ded.CalculationMethod,
                        DeductionType = 2,
                        Frequency = ded.Frequency,
                        IsAutomatic = ded.IsAutomatic,
                        IsPrenotification = ded.IsPrenotification,
                        LimitAmount = ded.LimitAmount,
                        PayDeductionId = ded.PayDeductionId
                    });
                }
            }

            IEnumerable<object> directDeposit = null;

            await Task.Run(() =>
            {
                directDeposit = from dd in eeDirDeps
                    join ee in eeList on dd.EmployeeId equals ee.Id
                    select new {EEId = ee.Id, EECode = ee.Code, dd.BankIdentifier, dd.BankAccountType, dd.BankAccount, dd.IsPrenotification, dd.Amount};
            });

            return directDeposit;
        }

        public async Task<string> SubscribeToNotification(SubscriptionMode Mode, string TypeId)
        {
            var results = "";

            var newReg = new CreateRegistration {Mode = Mode, TypeId = TypeId};

            try
            {
                results = (await processJsonObject<CreateRegistration>("PUT", "Api/Notification/V1/Registrations.svc/domain/registrations", newReg)).ToString();
                "Update Successful.".Dump();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cleanUp();
            }

            return results;
        }

        public async Task<object> GetNotificationRegistrations()
        {
            IList<Registration> results;

            try
            {
                results = (IList<Registration>) await processJsonObject<IList<Registration>>("GET", "Api/Notification/V1/Registrations.svc/domain/registrations");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cleanUp();
            }

            return results;
        }

        public async Task<object> GetAllChangeEvents(long Version = 0)
        {
            var results = new List<ChangeEvent>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<ChangeEvent>) await processJsonObject<List<ChangeEvent>>("GET", "Synchronization/EventService.svc/events/domain?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        #region Company

        public async Task<object> GetAllPayGroups()
        {
            var results = new List<PayGroup>();

            try
            {
                results = (List<PayGroup>) await processJsonObject<List<PayGroup>>("GET", "RemotePayroll/Api/Core/PayGroup.svc/paygroups");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cleanUp();
            }

            return results;
        }

        public async Task<object> GetAllUnions(long Version = 0)
        {
            var results = new List<Union>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<Union>) await processJsonObject<List<Union>>("GET", "RemotePayroll/Api/Core/Union.svc/unions?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllUnionClasses(long Version = 0)
        {
            var results = new List<UnionClass>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<UnionClass>) await processJsonObject<List<UnionClass>>("GET", "RemotePayroll/Api/Core/Union.svc/unions/locals/classes?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllUnionLocals(long Version = 0)
        {
            var results = new List<UnionLocal>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<UnionLocal>) await processJsonObject<List<UnionLocal>>("GET", "RemotePayroll/Api/Core/Union.svc/unions/locals?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }
            }

            return results;
        }

        public async Task<object> GetAllPayTypes(long Version = 0)
        {
            var results = new List<PayType>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<PayType>) await processJsonObject<List<PayType>>("GET", "RemotePayroll/Api/Pay/PayType.svc/paytypes?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllCertifiedClasses(long Version = 0)
        {
            var results = new List<CertifiedClass>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<CertifiedClass>) await processJsonObject<List<CertifiedClass>>("GET", "RemotePayroll/Api/Core/CertifiedClass.svc/certifiedclasses?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllDepartments(long Version = 0)
        {
            var results = new List<Department>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<Department>) await processJsonObject<List<Department>>("GET", "RemotePayroll/Api/Core/Department.svc/departments?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllDeductions(long Version = 0)
        {
            var results = new List<Deduction>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<Deduction>) await processJsonObject<List<Deduction>>("GET", "HumanResources/Api/ThirdParty/PayDeduction.svc/deductions?version=" + version);
                    //var verResults = (List<Deduction>)await processJsonObject<List<Deduction>>("GET", "HumanResources/Api/ThirdParty/PayDeduction.svc/deductions?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllPayTaxes(long Version = 0)
        {
            var results = new List<PayTax>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<PayTax>) await processJsonObject<List<PayTax>>("GET", "HumanResources/Api/ThirdParty/PayTax.svc/taxes?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetDeductionBankAccount(string DedID)
        {
            var results = new List<DedBankAccount>();

            try
            {
                results.Add((DedBankAccount) await processJsonObject<DedBankAccount>("GET", "HumanResources/Api/ThirdParty/EmployeeDeduction.svc/employee/deduction/bank-account?id=" + DedID));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cleanUp();
            }

            return results;
        }

        public async Task<object> GetDeductionBankIdentifier(string DedID)
        {
            var results = new List<DedBankIdentifier>();

            try
            {
                results.Add((DedBankIdentifier) await processJsonObject<DedBankIdentifier>("GET",
                    "HumanResources/Api/ThirdParty/EmployeeDeduction.svc/employee/deduction/bank-identifier?id=" + DedID));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cleanUp();
            }

            return results;
        }

        #endregion

        #region Employee

        public async Task<object> GetAllEmployees(long Version = 0)
        {
            var results = new List<Employee>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<Employee>) await processJsonObject<List<Employee>>("GET", "HumanResources/Api/ThirdParty/Employee.svc/employees?version=" + version);

                    if (verResults == null) break;

                    if (verResults.Any())
                    {
                        recordCount = verResults.Count();
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllEmployeePayTaxes(long Version = 0)
        {
            var results = new List<EEPayTax>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<EEPayTax>) await processJsonObject<List<EEPayTax>>("GET", "HumanResources/Api/ThirdParty/EmployeeTax.svc/employees/taxes?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllEmployeeStateTaxes(long Version = 0)
        {
            var results = new List<EEStateTax>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<EEStateTax>) await processJsonObject<List<EEStateTax>>("GET", "HumanResources/Api/ThirdParty/EmployeeStateTax.svc/employees/state-taxes?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllEmployeeDeductions(long Version = 0)
        {
            var results = new List<EEDeduction>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<EEDeduction>) await processJsonObject<List<EEDeduction>>("GET",
                        "HumanResources/Api/ThirdParty/EmployeeDeduction.svc/employees/deductions?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetAllEmployeePay(long Version = 0)
        {
            var results = new List<EEPay>();
            long version = Version;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<EEPay>) await processJsonObject<List<EEPay>>("GET", "HumanResources/Api/ThirdParty/EmployeeSalary.svc/employees/salaries?version=" + version);

                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        results = results.Concat(verResults).ToList();
                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetEEDedsByEEIdList(List<Employee> eeList)
        {
            var results = new List<EEDeduction>();
            long version = 0;
            int recordCount = 999999;
            int count = 0;

            while (recordCount >= _config.VersionBlockMax)
            {
                try
                {
                    var verResults = (List<EEDeduction>) await processJsonObject<List<EEDeduction>>("GET",
                        "HumanResources/Api/ThirdParty/EmployeeDeduction.svc/employees/deductions?version=" + version);
                    var hits = verResults.Where(ee => eeList.Any(t => t.Id == ee.EmployeeId));
                    recordCount = verResults.Count();
                    if (verResults.Any())
                    {
                        if (hits.Count() > 0) results = results.Concat(hits).ToList();

                        version = verResults.Last().Version;
                        count++;
                    }
                    else recordCount = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    cleanUp();
                }

                if (count > _config.MaxLoopCall)
                {
                    "MaxLoopCall Exceeded. Process cancelled.".Dump();
                    break;
                }
            }

            return results;
        }

        public async Task<object> GetEmployeeAddress(string EEId)
        {
            var results = new List<EEAddress>();

            try
            {
                results.Add((EEAddress) await processJsonObject<EEAddress>("GET", "HumanResources/Api/ThirdParty/Employee.svc/employee/street-address?id=" + EEId));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cleanUp();
            }

            return results;
        }

        public async Task<object> GetEmployeeBirthDate(string EEId)
        {
            var results = new List<EEBirthDate>();

            try
            {
                results.Add((EEBirthDate) await processJsonObject<EEBirthDate>("GET", "HumanResources/Api/ThirdParty/Employee.svc/employee/birth-date?id=" + EEId));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cleanUp();
            }

            return results;
        }

        public async Task<object> GetEmployeeSSN(string EEId)
        {
            var results = new List<EESSN>();

            try
            {
                results.Add((EESSN) await processJsonObject<EESSN>("GET", "HumanResources/Api/ThirdParty/Employee.svc/employee/ssn?id=" + EEId));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cleanUp();
            }

            return results;
        }

        private async Task<EEPII> UpdateEEPII(Employee ee)
        {
            var eePII = new EEPII();

            var task1 = GetEmployeeSSN(ee.Id);
            var task2 = GetEmployeeBirthDate(ee.Id);
            var task3 = GetEmployeeAddress(ee.Id);
            await Task.WhenAll(task1, task2, task3);

            var eeSSN = ((List<EESSN>) task1.Result).FirstOrDefault().SocialSecurityNumber.ToString();
            var eeBirthDate = ((List<EEBirthDate>) task2.Result).FirstOrDefault().DateOfBirth.ToString();
            var eeAddress = ((List<EEAddress>) task3.Result).FirstOrDefault();

            eePII.EEId = ee.Id;
            eePII.EECode = ee.Code;
            eePII.SSN = eeSSN;
            eePII.BirthDate = eeBirthDate;
            eePII.Address1 = eeAddress.Address1;
            eePII.Address2 = eeAddress.Address2;

            return eePII;
        }

        #endregion

        #region Helper Methods

        private void cleanUp()
        {
            //_config.Client.Dispose();
        }

        public static Dictionary<string, string> GetStaticConstantDictionary(Type t)
        {
            FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);
            var map = new Dictionary<string, string>();
            foreach (FieldInfo fi in fields)
            {
                map[fi.GetValue(null).ToString()] = fi.Name;
            }

            return map;
        }

        private static async Task<object> invokeMethod(Delegate method, params object[] args)
        {
            object results = null;
            await Task.Run(() =>
            {
                if (method.Method.ReturnType.IsSubclassOf(typeof(Task)))
                {
                    if (method.Method.ReturnType.IsConstructedGenericType)
                    {
                        dynamic tmp = method.DynamicInvoke(args);
                        results = tmp.GetAwaiter().GetResult();
                    }
                    else
                    {
                        (method.DynamicInvoke(args) as Task).GetAwaiter().GetResult();
                    }
                }
                else
                {
                    results = method.DynamicInvoke(args);
                }
            });

            return results;
        }

        private async Task<object> processJsonObject<Tobject>(string method, string appendURL, object obj = null)
        {
            string urlFull = $"{_config.BaseURI}";

            if (appendURL != "") urlFull += appendURL;
            if (_config.ShowEndpoint) urlFull.Dump();

            HttpResponseMessage response = new HttpResponseMessage();

            switch (method)
            {
                case "POST":
                    response = await _config.Client.PostAsJsonAsync(urlFull, obj).ConfigureAwait(false);
                    break;
                case "PUT":
                    response = await _config.Client.PutAsJsonAsync(urlFull, obj).ConfigureAwait(false);
                    break;
                case "DELETE":
                    response = await _config.Client.DeleteAsync(urlFull).ConfigureAwait(false);
                    break;
                default:
                    response = await _config.Client.GetAsync(urlFull).ConfigureAwait(false);
                    break;
            }

            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = (_config.ShowJson == JsonMode.AllRecords) ? MissingMemberHandling.Error : MissingMemberHandling.Ignore
            };

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(content))
                {
                    if (content.StartsWith("[") || content.StartsWith("{"))
                    {
                        if (_config.ShowJson == JsonMode.FirstRecord) content.Substring(0, content.IndexOf('}') + 1).Dump();
                        else if (_config.ShowJson == JsonMode.AllRecords) content.Dump();

                        return JsonConvert.DeserializeObject<Tobject>(content, jsonSettings);
                    }
                    else
                        return content;
                }
                else return Enumerable.Empty<dynamic>();
            }
            else
            {
                var error = response.Content.ReadAsStringAsync().Result;
                Util.RawHtml(error).Dump();
                return default(Tobject);
            }
        }

        private void writeToCSV(IEnumerable results, [CallerMemberName] string fileName = "")
        {
            if (_config.WriteToScreen)
            {
                results.Dump();
                return;
            }

            if (fileName == "") fileName = "results";

            if (Path.GetExtension(fileName) != ".csv")
            {
                fileName = $"{_config.HH2Subdomain}_{fileName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
            }

            var exportFullFilePath = Path.Combine(_config.ExportCSVPath, fileName);
            using (var writeCsvFile = new StreamWriter(exportFullFilePath))
            using (var csv = new CsvHelper.CsvWriter(writeCsvFile, CultureInfo.InvariantCulture))
            {
                try
                {
                    csv.WriteRecords(results);
                    $"CSV FILE CREATED: {exportFullFilePath}".Dump();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Setup

        public static async Task<HH2API> Create(AppSettings config)
        {
            var hh2 = new HH2API(config);
            await hh2.authenticate();
            return hh2;
        }

        private HH2API(AppSettings config)
        {
            _config = config;
            _config.BaseURI = $"https://{config.HH2Subdomain}.hh2.com/";
            
            var handler = new HttpClientHandler() {ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true};
            _config.Client = new HttpClient(handler) {BaseAddress = new Uri(_config.BaseURI)};
            _config.Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task authenticate()
        {
            var responseBody = "";
            var token = AuthenticationResult.NoResponse;

            var request = new
            {
                ApiKey = Util.GetPassword("hh2apikey"),
                ApiSecret = Util.GetPassword("hh2apisecret"),
                Username = Util.GetPassword("hh2username"),
                Password = Util.GetPassword(_config.HH2Subdomain)
            };

            var url = _config.BaseURI + "Api/Security/V3/Session.svc/authenticate";
            var response = _config.Client.PostAsJsonAsync(url, request);

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
        }

        #endregion
    }
}