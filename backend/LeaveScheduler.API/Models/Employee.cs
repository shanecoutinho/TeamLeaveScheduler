using System.ComponentModel.DataAnnotations;

namespace LeaveScheduler.API.Models;

public class Employee
{
    public int Id {get;set;}

    [Required]
    [MaxLength(100)]
    public string FullName {get;set;   } = string.Empty;
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string JobTitle { get; set; } = string.Empty;

    public int TeamId { get; set; }

    public Team Team { get; set; } = null!;

    public List<LeaveRequest> LeaveRequests { get; set; } = new();

}