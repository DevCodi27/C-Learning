namespace EmployeeManagement.Models;

public record SalaryCalculation
{
    public decimal Bonus { get; init; }
    public decimal Deductions { get; init; }
    public decimal NetSalary { get; init; }
}
