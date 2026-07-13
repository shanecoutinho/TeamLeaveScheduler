using LeaveScheduler.API.Models;

namespace LeaveScheduler.API.Data;

public static class DatabaseSeeder
{
    public static void Seed(AppDbContext context)

    {
    
        if (context.Teams.Any())
        {
            return;
        }

        var engineering = new Team
        {
            Name = "Engineering"
        };

        var finance = new Team
        {
            Name = "Finance"
        };

        var operations = new Team
        {
            Name = "Operations"
        };

        context.Teams.AddRange(engineering, finance, operations);
        context.SaveChanges();

        var employees = new List<Employee>
        {
            new Employee
            {
                FullName = "Shane Coutinho",
                PhoneNumber = "0771234567",
                JobTitle = "Software Developer",
                TeamId = engineering.Id
            },

            new Employee
            {
                FullName = "Kimberly Magoronga",
                PhoneNumber = "0772345678",
                JobTitle = "Backend Developer",
                TeamId = engineering.Id
            },

            new Employee
            {
                FullName = "Natasha Chiradza",
                PhoneNumber = "0773456789",
                JobTitle = "QA Engineer",
                TeamId = engineering.Id
            },

            new Employee
            {
                FullName = "Michael Mutarah",
                PhoneNumber = "0774567890",
                JobTitle = "Accountant",
                TeamId = finance.Id
            },

            new Employee
            {
                FullName = "Venice Yardley",
                PhoneNumber = "0775678901",
                JobTitle = "Operations Officer",
                TeamId = operations.Id
            },

            new Employee
            {
                FullName = "Tendai Jack",
                PhoneNumber = "0776789012",
                JobTitle = "Operations Supervisor",
                TeamId = operations.Id
            }
        };

        context.Employees.AddRange(employees);
        context.SaveChanges();
        
    }
}