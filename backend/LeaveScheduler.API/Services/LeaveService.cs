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
    // Check employee exists
    var employee = await _context.Employees.FindAsync(request.EmployeeId);

    if (employee == null)
    {
        throw new Exception("Employee not found.");
    }

    // Validate dates
    if (request.EndDate < request.StartDate)
    {
        throw new Exception("End date cannot be before the start date.");
    }

    // Check overlapping approved leave
    if (HasOverlappingLeave(employee.Id, request.StartDate, request.EndDate))
    {
        throw new Exception("Employee already has approved leave during this period.");
    }

    // Check team capacity
    if (IsTeamCapacityExceeded(employee.TeamId, request.StartDate, request.EndDate))
    {
        throw new Exception("Team leave capacity exceeded.");
    }

    // Calculate working days
    int workingDays = CalculateWorkingDays(request.StartDate, request.EndDate);

    if (workingDays <= 0)
    {
        throw new Exception("Leave request contains no working days.");
    }

    // Create leave request
    var leaveRequest = new LeaveRequest
    {
        EmployeeId = employee.Id,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        LeaveReason = request.LeaveReason,
        Status = LeaveStatus.Pending
    };

    _context.LeaveRequests.Add(leaveRequest);

    await _context.SaveChangesAsync();

    return leaveRequest;

}   
public async Task<LeaveRequest> ApproveLeaveRequest(int leaveRequestId)
{
    var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

    if (leaveRequest == null)
    {
        throw new Exception("Leave request not found.");
    }

    if (leaveRequest.Status != LeaveStatus.Pending)
    {
        throw new Exception("Only pending leave requests can be approved.");
    }

    leaveRequest.Status = LeaveStatus.Approved;

    await _context.SaveChangesAsync();

    return leaveRequest;
}

}