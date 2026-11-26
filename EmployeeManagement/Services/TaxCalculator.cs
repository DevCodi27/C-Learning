namespace EmployeeManagement.Services;

public static class TaxCalculator
{
    public static decimal CalculateAnnualTax(decimal grossSalary, bool isRemoteWorker)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(grossSalary);

        decimal taxRate = grossSalary switch
        {
            <= 30_000m => 0.10m,
            <= 80_000m => 0.20m,
            _          => 0.30m
        };

        decimal baseTax = grossSalary * taxRate;
        const decimal remoteWorkerTaxCredit = 1_500m;
        
        decimal finalTax = isRemoteWorker 
            ? Math.Max(0m, baseTax - remoteWorkerTaxCredit) 
            : baseTax;

        return finalTax;
    }
}
