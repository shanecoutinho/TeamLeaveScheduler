using LeaveScheduler.API.Data;
using LeaveScheduler.API.DTOs;
using LeaveScheduler.API.Models;
using LeaveScheduler.API.Models.Enums;
using LeaveScheduler.API.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LeaveScheduler.API.Tests;

public class LeaveServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void OverlapRule_ReturnsTrue_WhenEmployeeAlreadyHasApprovedLeave()
    {
        // Arrange
        var context = CreateContext();

        var team = new Team { Name = "Engineering" };
        context.Teams.Add(team);
        context.SaveChanges();

        var employee = new Employee
        {
            FullName = "Test User",
            PhoneNumber = "0770000000",
            JobTitle = "Developer",
            TeamId = team.Id
        };

        context.Employees.Add(employee);
        context.SaveChanges();

        context.LeaveRequests.Add(new LeaveRequest
        {
            EmployeeId = employee.Id,
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 5),
            LeaveReason = "Vacation",
            Status = LeaveStatus.Approved
        });

        context.SaveChanges();

        var holidayService = new PublicHolidayService();
        var service = new LeaveService(context, holidayService);

        // Act
        var result = service.HasOverlappingLeave(
            employee.Id,
            new DateOnly(2026, 8, 3),
            new DateOnly(2026, 8, 6));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TeamCapacity_ReturnsTrue_When30PercentRuleExceeded()
    {
        // Arrange
        var context = CreateContext();

        var team = new Team { Name = "Engineering" };
        context.Teams.Add(team);
        context.SaveChanges();

        var e1 = new Employee
        {
            FullName = "Emp1",
            PhoneNumber = "1",
            JobTitle = "Dev",
            TeamId = team.Id
        };

        var e2 = new Employee
        {
            FullName = "Emp2",
            PhoneNumber = "2",
            JobTitle = "Dev",
            TeamId = team.Id
        };

        var e3 = new Employee
        {
            FullName = "Emp3",
            PhoneNumber = "3",
            JobTitle = "Dev",
            TeamId = team.Id
        };

        context.Employees.AddRange(e1, e2, e3);
        context.SaveChanges();

        context.LeaveRequests.Add(new LeaveRequest
        {
            EmployeeId = e1.Id,
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 5),
            LeaveReason = "Vacation",
            Status = LeaveStatus.Approved
        });

        context.SaveChanges();

        var holidayService = new PublicHolidayService();
        var service = new LeaveService(context, holidayService);

        // Act
        var exceeded = service.IsTeamCapacityExceeded(
            team.Id,
            new DateOnly(2026, 8, 2),
            new DateOnly(2026, 8, 4));

        // Assert
        Assert.True(exceeded);
    }
}