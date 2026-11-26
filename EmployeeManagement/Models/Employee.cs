namespace EmployeeManagement.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; } = 0;
    public decimal? PerformanceBonus { get; set; } = null;

    public object this[string propertyName]
    {
        get
        {
            return propertyName.ToLower() switch
            {
                "name" => Name,
                "position" => Position,
                "department" => Department,
                "salary" => Salary,
                _ => throw new KeyNotFoundException($"No property {propertyName}")
            };
        }
        set
        {
            switch (propertyName.ToLower())
            {
                case "name":
                    Name = value?.ToString() ?? string.Empty;
                    break;
                case "position":
                    Position = value?.ToString() ?? string.Empty;
                    break;
                case "department":
                    Department = value?.ToString() ?? string.Empty;
                    break;
                case "salary":
                    Salary = Convert.ToDecimal(value);
                    break;
            }
        }
    }
}
