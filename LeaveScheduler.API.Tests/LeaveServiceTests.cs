using LeaveScheduler.API.Data;
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
    public void CalculateWorkingDays_MondayToFriday_ReturnsFive()
    {
        // Arrange
        var context = CreateContext();

        var holidayService = new PublicHolidayService(new HashSet<DateOnly>());

        var service = new LeaveService(context, holidayService);

        var start = new DateOnly(2026, 7, 20); // Monday
        var end = new DateOnly(2026, 7, 24);   // Friday

        // Act
        var result = service.CalculateWorkingDays(start, end);

        // Assert
        Assert.Equal(5, result);
    }
}