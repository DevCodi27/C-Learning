using EmployeeManagement.Models;
using EmployeeManagement.Services;

var employeeService = new EmployeeService();
bool isContinue = true;
List<IPayrollCalculable> payrollItems =
[
    new FullTimeEmployee(1, "Alice Johnson", 5000m, 10m),
    new Contractor("Acme Consulting", 75m) { HoursWorked = 160 }
];

while (isContinue)
{
    Console.WriteLine("================================");
    Console.WriteLine("EMPLOYEE MANAGEMENT SYSTEM");
    Console.WriteLine("1. Add Employee");
    Console.WriteLine("2. View All Employees");
    Console.WriteLine("3. Search Employee");
    Console.WriteLine("4. Update Employee");
    Console.WriteLine("5. Delete Employee");
    Console.WriteLine("6. Calculate Salary");
    Console.WriteLine("7. Calculate Annual Tax");
    Console.WriteLine("8. Polymorphic Payroll (FTE & Contractor)");
    Console.WriteLine("9. Exit");
    Console.Write("Enter your choice (1-9): ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        continue;
    }

    switch (choice)
    {
        case 1:
            AddEmployee(employeeService);
            break;
        case 2:
            ViewAllEmployees(employeeService);
            break;
        case 3:
            SearchEmployee(employeeService);
            break;
        case 4:
            UpdateEmployee(employeeService);
            break;
        case 5:
            DeleteEmployee(employeeService);
            break;
        case 6:
            CalculateSalary(employeeService);
            break;
        case 7:
            AnnualTax();
            break;
        case 8:
            RunPolymorphicPayroll(payrollItems);
            break;
        case 9:
            Console.WriteLine("Exiting the program. Goodbye!");
            isContinue = false;
            break;
        default:
            Console.WriteLine("Invalid choice. Please select a number between 1 and 9.");
            break;
    }
}

void RunPolymorphicPayroll(List<IPayrollCalculable> items)
{
    Console.WriteLine("\n--- Polymorphic Payroll (FTE & Contractor) ---");
    Console.WriteLine("1. View Payroll Summary");
    Console.WriteLine("2. Add Full-Time Employee");
    Console.WriteLine("3. Add Contractor");
    Console.Write("Enter option (1-3): ");

    string option = Console.ReadLine() ?? string.Empty;

    if (option == "2")
    {
        Console.Write("Enter Employee ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID."); return; }

        Console.Write("Enter Name: ");
        string name = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter Base Salary: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal baseSalary)) { Console.WriteLine("Invalid salary."); return; }

        Console.Write("Enter Bonus Percentage: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal bonusPct)) { Console.WriteLine("Invalid bonus percentage."); return; }

        try
        {
            items.Add(new FullTimeEmployee(id, name, baseSalary, bonusPct));
            Console.WriteLine("Full-time employee added to payroll!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        return;
    }

    if (option == "3")
    {
        Console.Write("Enter Company Name: ");
        string companyName = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter Hourly Rate: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal hourlyRate)) { Console.WriteLine("Invalid hourly rate."); return; }

        Console.Write("Enter Hours Worked: ");
        if (!int.TryParse(Console.ReadLine(), out int hoursWorked)) { Console.WriteLine("Invalid hours worked."); return; }

        try
        {
            var contractor = new Contractor(companyName, hourlyRate) { HoursWorked = hoursWorked };
            items.Add(contractor);
            Console.WriteLine("Contractor added to payroll!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        return;
    }

    if (option != "1")
    {
        Console.WriteLine("Invalid option.");
        return;
    }

    decimal totalPayout = 0m;

    foreach (var item in items)
    {
        decimal payout = item.CalculateMonthlyPayout();
        totalPayout += payout;

        string label = item switch
        {
            FullTimeEmployee fte => $"FTE #{fte.Id} {fte.Name}",
            Contractor c => $"Contractor ({c.CompanyName}, {c.HoursWorked} hrs)",
            _ => "Unknown"
        };

        Console.WriteLine($"{label} => Monthly Payout: {payout:C}");
    }

    Console.WriteLine($"Total Monthly Payout: {totalPayout:C}");
}

string GetDepartment()
{
    while (true)
    {
        Console.Write($"Enter Employee Department ({string.Join(", ", EmployeeService.ValidDepartments)}): ");
        string department = (Console.ReadLine() ?? string.Empty).Trim();
        string? matched = Array.Find(EmployeeService.ValidDepartments, d => d.Equals(department, StringComparison.OrdinalIgnoreCase));

        if (matched != null)
        {
            return matched;
        }

        Console.WriteLine("You entered an invalid department. Please choose from the list.");
    }
}

void AddEmployee(EmployeeService service)
{
    Console.WriteLine("\n--- Adding New Employee ---");

    Console.Write("Enter Employee Name: ");
    string name = Console.ReadLine() ?? string.Empty;

    Console.Write("Enter Employee Position: ");
    string position = Console.ReadLine() ?? string.Empty;

    string department = GetDepartment();

    Console.Write("Enter Employee Salary: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
    {
        Console.WriteLine("Invalid input for Salary.");
        return;
    }

    Console.Write("Enter Performance Bonus (press Enter to skip): ");
    string bonusInput = Console.ReadLine() ?? string.Empty;
    decimal? bonus = decimal.TryParse(bonusInput, out decimal parsedBonus) ? parsedBonus : null;

    service.Add(name, position, department, salary, bonus);
    Console.WriteLine("Employee added successfully!");
}

void ViewAllEmployees(EmployeeService service)
{
    Console.WriteLine("\n--- All Employees ---");
    var employees = service.GetAll();

    if (employees.Count == 0)
    {
        Console.WriteLine("No employees found.");
        return;
    }

    foreach (var emp in employees)
    {
        Console.WriteLine($"ID: {emp.Id} | Name: {emp.Name} | Position: {emp.Position} | Dept: {emp.Department} | Salary: {emp.Salary:C}");
    }
}

void SearchEmployee(EmployeeService service)
{
    Console.Write("\nEnter Employee ID to search: ");
    if (!int.TryParse(Console.ReadLine(), out int searchId))
    {
        Console.WriteLine("Invalid Employee ID.");
        return;
    }

    var emp = service.FindById(searchId);
    if (emp != null)
    {
        Console.WriteLine($"Found:\nID: {emp.Id} | Name: {emp.Name} | Position: {emp.Position} | Dept: {emp.Department} | Salary: {emp.Salary:C}");
    }
    else
    {
        Console.WriteLine("Employee not found.");
    }
}

void UpdateEmployee(EmployeeService service)
{
    Console.Write("\nEnter Employee ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out int updateId))
    {
        Console.WriteLine("Invalid Employee ID.");
        return;
    }

    var emp = service.FindById(updateId);
    if (emp == null)
    {
        Console.WriteLine("Employee not found.");
        return;
    }

    Console.Write($"Enter new Name (current: {emp.Name}, leave blank to keep): ");
    string newName = Console.ReadLine() ?? string.Empty;
    if (!string.IsNullOrWhiteSpace(newName)) emp.Name = newName;

    Console.Write($"Enter new Position (current: {emp.Position}, leave blank to keep): ");
    string newPos = Console.ReadLine() ?? string.Empty;
    if (!string.IsNullOrWhiteSpace(newPos)) emp.Position = newPos;

    Console.Write($"Enter new Department (current: {emp.Department}, leave blank to keep): ");
    string newDept = Console.ReadLine() ?? string.Empty;
    if (!string.IsNullOrWhiteSpace(newDept))
    {
        string? matched = Array.Find(EmployeeService.ValidDepartments, d => d.Equals(newDept.Trim(), StringComparison.OrdinalIgnoreCase));
        if (matched != null) emp.Department = matched;
        else Console.WriteLine("Invalid department entered, keeping current.");
    }

    Console.Write($"Enter new Salary (current: {emp.Salary}, leave blank to keep): ");
    string newSalaryInput = Console.ReadLine() ?? string.Empty;
    if (decimal.TryParse(newSalaryInput, out decimal newSalary))
    {
        emp.Salary = newSalary;
    }

    Console.WriteLine("Employee updated successfully!");
}

void DeleteEmployee(EmployeeService service)
{
    Console.Write("\nEnter Employee ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int deleteId))
    {
        Console.WriteLine("Invalid Employee ID.");
        return;
    }

    if (service.Delete(deleteId))
    {
        Console.WriteLine("Employee deleted successfully!");
    }
    else
    {
        Console.WriteLine("Employee not found.");
    }
}

void CalculateSalary(EmployeeService service)
{
    if (service.GetCount() == 0)
    {
        Console.WriteLine("No employees found.");
        return;
    }

    Console.WriteLine("\n1. Calculate Net Salary for an Individual Employee");
    Console.WriteLine("2. View Total and Average Payroll of all Employees");
    Console.Write("Enter option (1-2): ");
    string option = Console.ReadLine() ?? string.Empty;

    if (option == "1")
    {
        Console.Write("Enter Employee ID: ");
        if (!int.TryParse(Console.ReadLine(), out int empId))
        {
            Console.WriteLine("Invalid Employee ID.");
            return;
        }

        var emp = service.FindById(empId);
        if (emp == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        Console.Write("Enter Bonus Amount: ");
        decimal.TryParse(Console.ReadLine(), out decimal bonus);

        Console.Write("Enter Deduction Amount: ");
        decimal.TryParse(Console.ReadLine(), out decimal deductions);

        var breakdown = PayrollService.CalculateNetSalary(emp.Salary, bonus, deductions);
        Console.WriteLine($"\n--- Salary Breakdown for {emp.Name} (ID: {emp.Id}) ---");
        Console.WriteLine($"Base Salary:  {emp.Salary:C}");
        Console.WriteLine($"Bonus:       +{breakdown.Bonus:C}");
        Console.WriteLine($"Deductions:  -{breakdown.Deductions:C}");
        Console.WriteLine("----------------------------------");
        Console.WriteLine($"Net Salary:   {breakdown.NetSalary:C}");
    }
    else if (option == "2")
    {
        var (total, avg) = service.GetPayrollSummary();
        Console.WriteLine("\n--- Company Payroll Summary ---");
        Console.WriteLine($"Total Employees: {service.GetCount()}");
        Console.WriteLine($"Total Payroll:   {total:C}");
        Console.WriteLine($"Average Salary:  {avg:C}");
    }
    else
    {
        Console.WriteLine("Invalid option.");
    }
}

void AnnualTax()
{
    Console.Write("\nEnter Gross Salary: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal grossSalary))
    {
        Console.WriteLine("Error: Please enter a valid numeric salary.");
        return;
    }

    Console.Write("Are you a remote worker? (Yes/No): ");
    string userRes = Console.ReadLine() ?? string.Empty;
    bool isRemote = string.Equals(userRes.Trim(), "yes", StringComparison.OrdinalIgnoreCase);

    decimal tax = TaxCalculator.CalculateAnnualTax(grossSalary, isRemote);
    Console.WriteLine($"\nAnnual Tax Liability: {tax:C}\n");
}
