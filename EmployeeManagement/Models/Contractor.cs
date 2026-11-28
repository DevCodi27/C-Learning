namespace EmployeeManagement.Models;

public sealed class Contractor : IPayrollCalculable
{
    private int _hoursWorked;

    public string CompanyName { get; init; }
    public decimal HourlyRate { get; init; }

    public int HoursWorked
    {
        get => _hoursWorked;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _hoursWorked = value;
        }
    }

    public Contractor(string companyName, decimal hourlyRate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hourlyRate);

        CompanyName = companyName;
        HourlyRate = hourlyRate;
    }

    public decimal CalculateMonthlyPayout() => HourlyRate * HoursWorked;
}