using LeaveScheduler.API.Data;
using LeaveScheduler.API.Models;
using LeaveScheduler.API.Models.Enums;
using Microsoft.EntityFrameworkCore;
using LeaveScheduler.API.DTOs;  

namespace LeaveScheduler.API.Services;

public class LeaveService
{
    private readonly AppDbContext _context;
    private readonly PublicHolidayService _holidayService;

    public LeaveService(
        AppDbContext context,
        PublicHolidayService holidayService)
    {
        _context = context;
        _holidayService = holidayService;
    }

    public int CalculateWorkingDays(DateOnly startDate, DateOnly endDate)
    {
        if (endDate < startDate)
        {
            throw new ArgumentException("End date cannot be before the start date.");
        }

        int workingDays = 0;

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday &&
                !_holidayService.IsPublicHoliday(date))
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
    public bool IsTeamCapacityExceeded(
    int teamId,
    DateOnly startDate,
    DateOnly endDate)
{
    // Total employees in the team
    int totalEmployees = _context.Employees.Count(e => e.TeamId == teamId);

    // Maximum employees allowed on leave
    int maxAllowed = (int)Math.Ceiling(totalEmployees * 0.30);

    // Check every working day
    for (var date = startDate; date <= endDate; date = date.AddDays(1))
    {
        // Skip weekends
        if (date.DayOfWeek == DayOfWeek.Saturday ||
            date.DayOfWeek == DayOfWeek.Sunday)
        {
            continue;
        }

        // Skip public holidays
        if (_holidayService.IsPublicHoliday(date))
        {
            continue;
        }

        // Count employees already on approved leave for this day
        int employeesOnLeave = _context.LeaveRequests
            .Include(request => request.Employee)
            .Count(request =>
                request.Employee.TeamId == teamId &&
                request.Status == LeaveStatus.Approved &&
                request.StartDate <= date &&
                request.EndDate >= date);

        // Would approving another employee exceed the limit?
        if (employeesOnLeave >= maxAllowed)
        {
            return true;
        }
    }

    return false;
}

public async Task<LeaveRequest> SubmitLeaveRequest(
    CreateLeaveRequestDto request)
{
    throw new NotImplementedException();
}
}