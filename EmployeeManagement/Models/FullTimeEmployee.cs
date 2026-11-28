namespace EmployeeManagement.Models;

public sealed class FullTimeEmployee(int id, string name, decimal baseSalary, decimal bonusPercentage)
    : BaseEmployee(id, name, baseSalary)
{
    private readonly decimal _bonusPercentage = ValidateBonus(bonusPercentage);

    public decimal BonusPercentage
    {
        get => _bonusPercentage;
        init => _bonusPercentage = ValidateBonus(value);
    }

    private static decimal ValidateBonus(decimal value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        return value;
    }

    public override decimal CalculateMonthlyPayout() =>
        BaseSalary + (BaseSalary * (BonusPercentage / 100m));
}