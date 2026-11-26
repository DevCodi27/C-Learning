using EmployeeManagement.Models;

namespace EmployeeManagement.Services;

public class EmployeeService
{
    private readonly List<Employee> _employees = new();
    private int _nextId = 0;

    public static readonly string[] ValidDepartments = ["HR", "IT", "Finance", "Sales", "Marketing"];

    public IReadOnlyList<Employee> GetAll() => _employees.AsReadOnly();

    public int GetCount() => _employees.Count;

    public void Add(string name, string position, string department, decimal salary, decimal? performanceBonus)
    {
        _nextId++;
        var newEmployee = new Employee
        {
            Id = _nextId,
            Name = name,
            Position = position,
            Department = department,
            Salary = salary,
            PerformanceBonus = performanceBonus
        };
        _employees.Add(newEmployee);
    }

    public Employee? FindById(int id) => _employees.Find(e => e.Id == id);

    public bool Delete(int id)
    {
        var emp = FindById(id);
        if (emp is null) return false;
        _employees.Remove(emp);
        return true;
    }

    public (decimal TotalPayroll, decimal AverageSalary) GetPayrollSummary()
    {
        if (_employees.Count == 0) return (0m, 0m);

        decimal total = 0m;
        foreach (var emp in _employees)
        {
            total += emp.Salary;
        }

        decimal average = total / _employees.Count;
        return (total, average);
    }
}
