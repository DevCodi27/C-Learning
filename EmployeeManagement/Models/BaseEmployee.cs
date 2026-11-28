namespace EmployeeManagement.Models;

public abstract class BaseEmployee : IPayrollCalculable
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal BaseSalary { get; init; }

    protected BaseEmployee(int id, string name, decimal baseSalary)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id); 
        ArgumentOutOfRangeException.ThrowIfNegative(baseSalary);

        Id = id;
        Name = name;
        BaseSalary = baseSalary;

    }

    public abstract decimal CalculateMonthlyPayout();
}