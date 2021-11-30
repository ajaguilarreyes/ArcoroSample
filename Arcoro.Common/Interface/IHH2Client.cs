using System.Collections.Generic;
using Arcoro.Common.Model.Company;
using Arcoro.Common.Model.Employee;
using Arcoro.Common.Model.Notification;

namespace Arcoro.Common.Interface
{
    public interface IHH2Client
    {
        List<Employee> GetAllEmployees(long Version = 0);
        List<EESSN> GetEmployeeSSN(string EEId);
        object GetEmployeeById();
        List<EEAddress> GetEmployeeAddress(string EEId);
        List<EEBirthDate> GetEmployeeBirthDate(string EEId);
        object GetAllPaychecks();
        object GetAllPaycheckDeductions();
        object GetAllDeductionsTest();
        object GetAllPaycheckFringes();
        object GetJobsByStatus();
        object GetJobByJobID();
        List<EEDeduction> GetAllEmployeeDeductions(long Version = 0);
        object GetAllEmployeeFringes();
        List<EEPay> GetAllEmployeePay(long Version = 0);
        object GetAllTaxGroups();
        object GetAllEmployeeStateTaxes(List<Employee> eeList, List<EEPayTax> eePayTaxes);
        List<EEPayTax> GetAllEmployeePayTaxes(long Version = 0);
        object GetAllEmployeeTime();
        object GetAllSyncErrors();
        object GetAllStandardCostCodes();
        List<CertifiedClass> GetAllCertifiedClasses(long Version = 0);
        object GetAllFringes();
        List<Deduction> GetAllDeductions(long Version = 0);
        List<Department> GetAllDepartments(long Version = 0);
        object GetAllEmploymentStatuses();
        object GetAllEmploymentTypes();
        object GetAllJobs();
        List<PayGroup> GetAllPayGroups();
        List<PayTax> GetAllPayTaxes(long Version = 0);
        List<PayType> GetAllPayTypes(long Version = 0);
        List<Union> GetAllUnions(long Version = 0);
        List<UnionClass> GetAllUnionClasses(long Version = 0);
        List<UnionLocal> GetAllUnionLocals(long Version = 0);
        IList<Registration> GetNotificationRegistrations();
        object GetChangeEvents();
    }
}
