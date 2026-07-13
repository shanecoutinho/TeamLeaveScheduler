using LeaveScheduler.API.Data;
using LeaveScheduler.API.Models;
using LeaveScheduler.API.Models.Enums;

namespace LeaveScheduler.API.Services;

public class LeaveService
{
    private readonly AppDbContext _context;

    public LeaveService(AppDbContext context)
    {
        _context = context;
    }

    
    public int CalculateWorkingDays(DateOnly startDate, DateOnly endDate)
{   if (endDate < startDate)
{
    throw new ArgumentException("End date cannot be before the start date.");
}
    int workingDays = 0;

    for (var date = startDate; date <= endDate; date = date.AddDays(1))
    {
        if (date.DayOfWeek != DayOfWeek.Saturday &&
            date.DayOfWeek != DayOfWeek.Sunday)
        {
            workingDays++;
        }
    }

    return workingDays;
    

}
    public bool HasOverlappingLeave(
    int employeeId,
    DateOnly startDate,
    DateOnly endDate)
{
    return _context.LeaveRequests.Any(request =>
        request.EmployeeId == employeeId &&
        request.Status == LeaveStatus.Approved &&
        startDate <= request.EndDate &&
        endDate >= request.StartDate
    );
}
}   