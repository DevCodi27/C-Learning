using EmployeeManagement.Models;

namespace EmployeeManagement.Services;

public static class PayrollService
{
    public static SalaryCalculation CalculateNetSalary(decimal baseSalary, decimal bonus, decimal deductions)
    {
        decimal netSalary = baseSalary + bonus - deductions;

        return new SalaryCalculation
        {
            Bonus = bonus,
            Deductions = deductions,
            NetSalary = netSalary
        };
    }
}
