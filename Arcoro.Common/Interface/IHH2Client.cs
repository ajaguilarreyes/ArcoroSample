namespace Arcoro.Common.Interface
{
    public interface IHH2Client
    {
        object GetAllEmployees();
        object GetEmployeeSSN();
        object GetEmployeeById();
        object GetEmployeeAddress();
        object GetEmployeeBirthDate();
        object GetAllPaychecks();
        object GetAllPaycheckDeductions();
        object GetAllDeductionsTest();
        object GetAllPaycheckFringes();
        object GetJobsByStatus();
        object GetJobByJobID();
        object GetAllEmployeeDeductions();
        object GetAllEmployeeFringes();
        object GetAllEmployeePay();
        object GetAllTaxGroups();
        object GetAllEmployeeStateTaxes();
        object GetAllEmployeePayTaxes();
        object GetAllEmployeeTime();
        object GetAllSyncErrors();
        object GetAllStandardCostCodes();
        object GetAllCertifiedClasses();
        object GetAllFringes();
        object GetAllDeductions();
        object GetAllDepartments();
        object GetAllEmploymentStatuses();
        object GetAllEmploymentTypes();
        object GetAllJobs();
        object GetAllPayGroups();
        object GetAllPayTaxes();
        object GetAllPayTypes();
        object GetAllUnions();
        object GetAllUnionClasses();
        object GetAllUnionLocals();
        object GetNotificationRegistrations();
        object GetChangeEvents();
    }
}
