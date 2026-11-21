string[] validDepartment = new string[] { "HR", "IT", "Finance", "Sales", "Marketing" };
int employeeId = 0;
Console.WriteLine("================================");
Console.WriteLine("EMPLOYEE MANAGEMENT SYSTEM");
Console.WriteLine("1. Add Employee");
Console.WriteLine("2. View All Employees");
Console.WriteLine("3. Search Employee");
Console.WriteLine("4. Update Employee");
Console.WriteLine("5. Delete Employee");
Console.WriteLine("6. Calculate Salary");
Console.WriteLine("7. Exit");
List<Employee> employees = new List<Employee>();

int choice = 0;
bool isContinue = true;

Address add1 = new Address{City  = "Chennai"};
Address add2 = add1;

 add2.City = "Madurai";                                                                                                                          
                                                                                                                                                      
Console.WriteLine($"add1 City: {add1.City}"); // Prints: Chennai                                                                                  
Console.WriteLine($"add2 City: {add2.City}");
while (isContinue)
{
  Console.Write("Enter your choice (1-7): ");
  if (!int.TryParse(Console.ReadLine(), out choice))
  {
    Console.WriteLine("Invalid input. Please enter a valid number.");
  }

  switch (choice)
  {
    case 1:
      AddEmployee();
      break;
    case 2:
      ViewAllEmployees();
      break;
    case 3:
      SearchEmployee();
      break;
    case 4:
      UpdateEmployee();
      break;
    case 5:
      DeleteEmployee();
      break;
    case 6:
      CalculateSalary();
      break;
    case 7:
      Console.WriteLine("Exiting the program. Goodbye!");
      isContinue = false;
      break;
    default:
      Console.WriteLine("Invalid choice. Please select a number between 1 and 7.");
      break;
  }
}

string GetDepartment()
{
  bool isValidDepartment = false;
  string department;
  do
  {
    Console.Write($"Enter Employee Department ({string.Join(", ", validDepartment)}): ");
    department = (Console.ReadLine() ?? string.Empty).Trim();
    string? formattedDept = Array.Find(validDepartment, d => d.Equals(department, StringComparison.OrdinalIgnoreCase));
    if (formattedDept != null)
    {
      department = formattedDept; // Formatted to match the array                                                                                   
      isValidDepartment = true;
    }

    if (!isValidDepartment)
    {
      Console.WriteLine("You Entered Invalid Department");
    }
  } while (!isValidDepartment);

  return department;
}

void AddEmployee()
{
  Console.WriteLine("Adding a new employee...");
  employeeId++;

  Console.Write("Enter Employee Name: ");
  string name = Console.ReadLine() ?? string.Empty;

  Console.Write("Enter Employee Position: ");
  string position = Console.ReadLine() ?? string.Empty;

  string department = GetDepartment();

  Console.Write("Enter Employee Salary: ");
  decimal salary = 0;
  if (!decimal.TryParse(Console.ReadLine(), out salary))
  {
    Console.WriteLine("Invalid input for Salary.");
    return;
  }

  Console.Write("Enter Performance Bonus(press Enter to skip): ");
  string bonusInput = Console.ReadLine() ?? string.Empty;

  decimal? bonus = null;
  if (decimal.TryParse(bonusInput, out decimal parsedBonus))                                              
  {                                                                                                                                                 
      bonus = parsedBonus;                                                                                                                          
  }  

  Employee newEmployee = new Employee
  {
    Id = employeeId,
    Name = name,
    Position = position,
    Department = department,
    Salary = salary,
    PerformanceBonus = bonus
  };
  employees.Add(newEmployee);
  Console.WriteLine("Employee added successfully!");
}

void ViewAllEmployees()
{
  Console.WriteLine("Viewing all employees...");
  if (employees.Count == 0)
  {
    Console.WriteLine("No employees found.");
    return;
  }
  foreach (Employee emp in employees)
  {
    Console.WriteLine($"ID: {emp.Id},\nName: {emp.Name},\nPosition: {emp.Position},\nDepartment: {emp.Department},\nSalary: {emp.Salary}");
  }
}

void SearchEmployee()
{
  Console.WriteLine("Searching for an employee...");
  Employee? foundEmployee = null;
  Console.Write("Enter Employee ID to search: ");
  int searchId = 0;
  if (!int.TryParse(Console.ReadLine(), out searchId))
  {
    Console.WriteLine("Invalid input for Employee ID.");
    return;
  }
  foundEmployee = employees.Find(emp => emp.Id == searchId);
  if (foundEmployee != null)
  {
    Console.WriteLine($"Employee found:\nID: {foundEmployee.Id},\nName: {foundEmployee.Name},\nPosition: {foundEmployee.Position},\nDepartment: {foundEmployee.Department},\nSalary: {foundEmployee.Salary}");
  }
  else
  {
    Console.WriteLine("Employee not found.");
  }
}

void UpdateEmployeeData(ref Employee employee, string key, object value) {
  if (value is string data && !string.IsNullOrWhiteSpace(data) )
  {
    employee[key] = data;
  }

  if ( value is decimal salary)
  {
    employee.Salary = salary;
  }
}

void UpdateEmployee()
{
  Console.WriteLine("Updating an employee...");
  int updateId = 0;
  if (!int.TryParse(Console.ReadLine(), out updateId))
  {
    Console.WriteLine("Invalid input for Employee ID.");
    return;
  }
  Employee? employeeToUpdate = employees.Find(emp => emp.Id == updateId);

  if(employeeToUpdate == null )
  {
    Console.WriteLine("Employee not found.");
    return;
  }

  Console.Write("Enter new Name (leave blank to keep current): ");
  string newName = Console.ReadLine() ?? string.Empty;
  UpdateEmployeeData(ref employeeToUpdate, "Name", newName);

  Console.Write("Enter new Position (leave blank to keep current): ");
  string newPosition = Console.ReadLine() ?? string.Empty;
  UpdateEmployeeData(ref employeeToUpdate, "Position", newPosition);


  Console.Write("Enter new Department (leave blank to keep current): ");
  string newDepartment = Console.ReadLine() ?? string.Empty;
  UpdateEmployeeData(ref employeeToUpdate, "Department", newDepartment);


  Console.Write("Enter new Salary (leave blank to keep current): ");                                                                                
  string input = Console.ReadLine() ?? string.Empty;                                                                                                
                                                                                                                                                    
  // Only update if the user actually typed a valid number!                                                                                         
  if (decimal.TryParse(input, out decimal newSalary))                                                                                               
  {                                                                                                                                                 
    UpdateEmployeeData(ref employeeToUpdate, "Salary", newSalary);                                                                                
  };

  Console.WriteLine("Employee updated successfully!");
}

void DeleteEmployee()
{
  Console.WriteLine("Deleting an employee...");
  Console.Write("Enter Employee ID to delete: ");
  int deleteId = 0;
  if (!int.TryParse(Console.ReadLine(), out deleteId))
  {
    Console.WriteLine("Invalid input for Employee ID.");
    return;
  }

  Employee? employeeToDelete = employees.Find(emp => emp.Id == deleteId);
  if (employeeToDelete != null)
  {
    employees.Remove(employeeToDelete);
    Console.WriteLine("Employee deleted successfully!");
  }
  else
  {
    Console.WriteLine("Employee not found.");
  }
}

void CalculateSalary()
{
  Console.WriteLine("Calculating Salary...");
  if (employees.Count == 0)
  {
    Console.WriteLine("No employees found.");
    return;
  }

  Console.WriteLine("1. Calculate Net Salary for an Individual Employee (with bonus/deductions)");
  Console.WriteLine("2. View Total and Average Payroll of all Employees");
  Console.Write("Enter option (1-2): ");
  string option = Console.ReadLine() ?? string.Empty;

  if (option == "1")
  {
    Console.Write("Enter Employee ID: ");
    int empId = 0;
    if (!int.TryParse(Console.ReadLine(), out empId))
    {
      Console.WriteLine("Invalid input for Employee ID.");
      return;
    }

    Employee? emp = employees.Find(e => e.Id == empId);
    if (emp == null)
    {
      Console.WriteLine("Employee not found.");
      return;
    }
    DisplayNetSalary(emp);

  }
  else if (option == "2")
  {
    DisplayTotalSalary();
  }
  else
  {
    Console.WriteLine("Invalid option.");
  }
}

void DisplayTotalSalary()
{
  decimal totalSalary = 0;
  foreach (Employee emp in employees)
  {
    totalSalary += emp.Salary;
  }
  decimal averageSalary = totalSalary / employees.Count;

  Console.WriteLine("\n--- Company Payroll Summary ---");
  Console.WriteLine($"Total Employees: {employees.Count}");
  Console.WriteLine($"Total Payroll:   {totalSalary}");
  Console.WriteLine($"Average Salary:  {averageSalary:F2}");
}

void DisplayNetSalary(Employee Employee)
{
  Console.WriteLine($"Base Salary: {Employee.Salary}");
  var salaryBreakDown = NetSalaryCalculation(Employee.Salary);
  Console.WriteLine($"\n--- Salary Breakdown for {Employee.Name} (ID: {Employee.Id}) ---");
  Console.WriteLine($"Base Salary:  {Employee.Salary}");
  Console.WriteLine($"Bonus:       +{salaryBreakDown.Bonus}");
  Console.WriteLine($"Deductions:  -{salaryBreakDown.Deductions}");
  Console.WriteLine($"---------------------------");
  Console.WriteLine($"Net Salary:   {salaryBreakDown.NetSalary}");
}

SalaryCalculation NetSalaryCalculation(decimal Salary)
{
  decimal bonus;
  decimal deductions;

  Console.Write("Enter Bonus Amount: ");
  decimal.TryParse(Console.ReadLine(), out bonus);

  Console.Write("Enter Deductin Amount: ");
  decimal.TryParse(Console.ReadLine(), out deductions);

  decimal netSalary = Salary + bonus - deductions;

  return new SalaryCalculation
  {
    Bonus = bonus,
    Deductions = deductions,
    NetSalary = netSalary
  };

}


public record SalaryCalculation
{
  public decimal Bonus { get; init; }
  public decimal Deductions { get; init; }
  public decimal NetSalary { get; init; }
}

public class Employee
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Position { get; set; } = string.Empty;
  public string Department { get; set; } = string.Empty;
  public decimal Salary { get; set; } = 0;

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
      switch(propertyName.ToLower())
      {
        case "name":
          Name = value.ToString() ?? string.Empty;
          break;
        case "position":
          Position = value.ToString() ?? string.Empty;
          break;
        case "department":
          Department = value.ToString() ?? string.Empty;
          break;
        case "salary":
          Salary = Convert.ToDecimal(value);
          break;
      }
    }
  }
  public decimal? PerformanceBonus {get; set;} = null;
}

public struct Address { public string City { get; set; } }
