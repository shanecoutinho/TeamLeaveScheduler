using System.ComponentModel.DataAnnotations;

namespace LeaveScheduler.API.DTOs;

public class CreateLeaveRequestDto
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [Required]
    [MaxLength(200)]
    public string LeaveReason { get; set; } = string.Empty;
}