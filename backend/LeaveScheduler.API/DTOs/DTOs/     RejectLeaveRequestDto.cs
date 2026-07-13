using System.ComponentModel.DataAnnotations;

namespace LeaveScheduler.API.DTOs;

public class RejectLeaveRequestDto
{
    [Required]
    [MaxLength(200)]
    public string RejectionReason { get; set; } = string.Empty;
}