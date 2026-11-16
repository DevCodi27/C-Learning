using System;
using System.Collections;

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

while (isContinue) {
Console.Write("Enter your choice (1-7): ");
if (!int.TryParse(Console.ReadLine(), out choice)) {
    Console.WriteLine("Invalid input. Please enter a valid number.");
}

switch (choice) {
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

 void AddEmployee() {
    Console.WriteLine("Adding a new employee...");
    Console.Write("Enter Employee ID: ");
    int id = 0;
    if(!int.TryParse(Console.ReadLine(), out id)) {
        Console.WriteLine("Invalid input for Employee ID.");
        return;
    }
    Console.Write("Enter Employee Name: ");
    string name = Console.ReadLine() ?? string.Empty;
    Console.Write("Enter Employee Position: ");
    string position = Console.ReadLine() ?? string.Empty;
    Console.Write("Enter Employee Department: ");
    string department = Console.ReadLine() ?? string.Empty;
    Console.Write("Enter Employee Salary: ");
    decimal salary = 0;
    if(!decimal.TryParse(Console.ReadLine(), out salary)) {
        Console.WriteLine("Invalid input for Salary.");
        return;
    }

    Employee newEmployee = new Employee {
        Id = id,
        Name = name,
        Position = position,
        Department = department,
        Salary = salary
    };
    employees.Add(newEmployee);
    Console.WriteLine("Employee added successfully!");
}

 void ViewAllEmployees() {
    Console.WriteLine("Viewing all employees...");
    if (employees.Count == 0) {
        Console.WriteLine("No employees found.");
        return;
    }
    foreach (Employee emp in employees) {
        Console.WriteLine($"ID: {emp.Id},\nName: {emp.Name},\nPosition: {emp.Position},\nDepartment: {emp.Department},\nSalary: {emp.Salary}");
    }
}

 void SearchEmployee() {
    Console.WriteLine("Searching for an employee...");
    Employee? foundEmployee = null;
    Console.Write("Enter Employee ID to search: ");
    int searchId = 0;
    if(!int.TryParse(Console.ReadLine(), out searchId)) {
        Console.WriteLine("Invalid input for Employee ID.");
        return;
    }
    foundEmployee = employees.Find(emp => emp.Id == searchId);
    if (foundEmployee != null) {
        Console.WriteLine($"Employee found:\nID: {foundEmployee.Id},\nName: {foundEmployee.Name},\nPosition: {foundEmployee.Position},\nDepartment: {foundEmployee.Department},\nSalary: {foundEmployee.Salary}");
    } else {
        Console.WriteLine("Employee not found.");
    }
}

void UpdateEmployee() {
    Console.WriteLine("Updating an employee...");
    Console.Write("Enter Employee ID to update: ");
    int updateId = 0;
    if(!int.TryParse(Console.ReadLine(), out updateId)) {
        Console.WriteLine("Invalid input for Employee ID.");
        return;
    }
    Employee? employeeToUpdate = employees.Find(emp => emp.Id == updateId);
    if (employeeToUpdate != null) {
        Console.Write("Enter new Name (leave blank to keep current): ");
        string newName = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(newName)) {
            employeeToUpdate.Name = newName;
        }

        Console.Write("Enter new Position (leave blank to keep current): ");
        string newPosition = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(newPosition)) {
            employeeToUpdate.Position = newPosition;
        }

        Console.Write("Enter new Department (leave blank to keep current): ");
        string newDepartment = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(newDepartment)) {
            employeeToUpdate.Department = newDepartment;
        }

        Console.Write("Enter new Salary (leave blank to keep current): ");
        string salaryInput = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(salaryInput) && decimal.TryParse(salaryInput, out decimal newSalary)) {
            employeeToUpdate.Salary = newSalary;
        }

        Console.WriteLine("Employee updated successfully!");
    } else {
        Console.WriteLine("Employee not found.");
    }
}

void DeleteEmployee() {
    Console.WriteLine("Deleting an employee...");
    Console.Write("Enter Employee ID to delete: ");
    int deleteId = 0;
    if (!int.TryParse(Console.ReadLine(), out deleteId)) {
        Console.WriteLine("Invalid input for Employee ID.");
        return;
    }

    Employee? employeeToDelete = employees.Find(emp => emp.Id == deleteId);
    if (employeeToDelete != null) {
        employees.Remove(employeeToDelete);
        Console.WriteLine("Employee deleted successfully!");
    } else {
        Console.WriteLine("Employee not found.");
    }
}

void CalculateSalary() {
    Console.WriteLine("Calculating Salary...");
    if (employees.Count == 0) {
        Console.WriteLine("No employees found.");
        return;
    }

    Console.WriteLine("1. Calculate Net Salary for an Individual Employee (with bonus/deductions)");
    Console.WriteLine("2. View Total and Average Payroll of all Employees");
    Console.Write("Enter option (1-2): ");
    string option = Console.ReadLine() ?? string.Empty;

    if (option == "1") {
        Console.Write("Enter Employee ID: ");
        int empId = 0;
        if (!int.TryParse(Console.ReadLine(), out empId)) {
            Console.WriteLine("Invalid input for Employee ID.");
            return;
        }

        Employee? emp = employees.Find(e => e.Id == empId);
        if (emp != null) {
            Console.WriteLine($"Base Salary: {emp.Salary}");
            Console.Write("Enter Bonus amount (press Enter for 0): ");
            decimal.TryParse(Console.ReadLine(), out decimal bonus);

            Console.Write("Enter Deductions amount (press Enter for 0): ");
            decimal.TryParse(Console.ReadLine(), out decimal deductions);

            decimal netSalary = emp.Salary + bonus - deductions;
            Console.WriteLine($"\n--- Salary Breakdown for {emp.Name} (ID: {emp.Id}) ---");
            Console.WriteLine($"Base Salary:  {emp.Salary}");
            Console.WriteLine($"Bonus:       +{bonus}");
            Console.WriteLine($"Deductions:  -{deductions}");
            Console.WriteLine($"---------------------------");
            Console.WriteLine($"Net Salary:   {netSalary}");
        } else {
            Console.WriteLine("Employee not found.");
        }
    } else if (option == "2") {
        decimal totalSalary = 0;
        foreach (Employee emp in employees) {
            totalSalary += emp.Salary;
        }
        decimal averageSalary = totalSalary / employees.Count;

        Console.WriteLine("\n--- Company Payroll Summary ---");
        Console.WriteLine($"Total Employees: {employees.Count}");
        Console.WriteLine($"Total Payroll:   {totalSalary}");
        Console.WriteLine($"Average Salary:  {averageSalary:F2}");
    } else {
        Console.WriteLine("Invalid option.");
    }
}

public class Employee {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; } = 0;
}
