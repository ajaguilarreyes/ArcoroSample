using System.Collections.Generic;
using Arcoro.Common.Interface;
using Arcoro.Common.Model.Company;
using Arcoro.Common.Model.Employee;
using Arcoro.Common.Model.Notification;

namespace Arcoro.Business.HttpClient
{
    public class HH2Client : IHH2Client
    {
        public HH2Client()
        {

        }

        List<CertifiedClass> IHH2Client.GetAllCertifiedClasses(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        List<Deduction> IHH2Client.GetAllDeductions(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllDeductionsTest()
        {
            throw new System.NotImplementedException();
        }

        List<Department> IHH2Client.GetAllDepartments(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        List<EEDeduction> IHH2Client.GetAllEmployeeDeductions(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllEmployeeFringes()
        {
            throw new System.NotImplementedException();
        }

        List<EEPay> IHH2Client.GetAllEmployeePay(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        List<EEPayTax> IHH2Client.GetAllEmployeePayTaxes(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        List<Employee> IHH2Client.GetAllEmployees(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllEmployeeStateTaxes(List<Employee> eeList, List<EEPayTax> eePayTaxes)
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllEmployeeTime()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllEmploymentStatuses()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllEmploymentTypes()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllFringes()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllJobs()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllPaycheckDeductions()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllPaycheckFringes()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllPaychecks()
        {
            throw new System.NotImplementedException();
        }

        List<PayGroup> IHH2Client.GetAllPayGroups()
        {
            throw new System.NotImplementedException();
        }

        List<PayTax> IHH2Client.GetAllPayTaxes(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        List<PayType> IHH2Client.GetAllPayTypes(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllStandardCostCodes()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllSyncErrors()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetAllTaxGroups()
        {
            throw new System.NotImplementedException();
        }

        List<UnionClass> IHH2Client.GetAllUnionClasses(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        List<UnionLocal> IHH2Client.GetAllUnionLocals(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        List<Union> IHH2Client.GetAllUnions(long Version = 0)
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetChangeEvents()
        {
            throw new System.NotImplementedException();
        }

        List<EEAddress> IHH2Client.GetEmployeeAddress(string EEId)
        {
            throw new System.NotImplementedException();
        }

        List<EEBirthDate> IHH2Client.GetEmployeeBirthDate(string EEId)
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetEmployeeById()
        {
            throw new System.NotImplementedException();
        }

        List<EESSN> IHH2Client.GetEmployeeSSN(string EEId)
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetJobByJobID()
        {
            throw new System.NotImplementedException();
        }

        object IHH2Client.GetJobsByStatus()
        {
            throw new System.NotImplementedException();
        }

        IList<Registration> IHH2Client.GetNotificationRegistrations()
        {
            throw new System.NotImplementedException();
        }
    }
}
