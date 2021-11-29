namespace Arcoro.Common.Model.Enum
{
    public enum AuthenticationResult
    {
        NoResponse,
        InvalidUserCredentials,
        InvalidWebApiClientCredentials,
        UserAccountLocked,
        WebApiClientLocked,
        Validated
    }

    public enum Gender
    {
        Male = 1,
        Female
    }

    public enum Ethnicity
    {
        Caucasian = 1,
        Black,
        Hispanic,
        Asian,
        AmericanIndian,
        Other,
        Hawaiian,
        AlaskanNative,
        PacificIslander,
        TwoOrMoreRaces
    }

    public enum MaritalStatus
    {
        Single = 1,
        Married,
        Divorced,
        Widowed
    }

    public enum PayFrequency
    {
        None,
        Weekly,
        Biweekly,
        SemiMonthly,
        Monthly,
        SemiAnnually,
        Annually,
        Check,
        OnDemand,
        Quarterly,
    }

    public enum LimitPeriod
    {
        Check = 1,
        MonthToDate,
        QuarterToDate,
        YearToDate,
        NoPeriod,
    }

    public enum BankAccountType
    {
        Checking = 1,
        Savings
    }

    public enum CalculationMethod
    {
        FlatAmount = 1,
        RegularHours,
        OvertimeHours,
        TotalHours,
        RegularPay,
        OvertimePay,
        TotalPay,
        StraightPay,
        NetPay,
        GrossPay,
        DaysWorked,
        WeeksWorked,
        OtherUnits,
        Formula,
        Default,
        HoursPaid,
    }

    public enum DeductionType
    {
        Normal = 1,
        DirectDeposit,
        NetDirectDeposit
    }

    public enum FilingStatus
    {
        None,
        Single,
        HeadOfHousehold,
        Married,
        NonResidentAlien
    }

    public enum AdjustmentMethod
    {
        None,
        NoTax,
        Exempt,
        Addon,
        AddonPercent,
        Replace,
        ReplacePercent,
        Formula
    }

    public enum SubscriptionMode
    {
        PerType = 1,
        PerRootEntity
    }

}