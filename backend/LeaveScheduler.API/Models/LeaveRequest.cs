        using System.ComponentModel.DataAnnotations;
using LeaveScheduler.API.Models.Enums;

namespace LeaveScheduler.API.Models;

public class LeaveRequest
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    [Required]
    [MaxLength(200)]
    public string LeaveReason { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? RejectionReason { get; set; }
}