using LeaveScheduler.API.Models;

using Microsoft.EntityFrameworkCore ;
  
namespace LeaveScheduler.API.Data;

public class AppDbContext : DbContext
    {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Team> Teams { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<LeaveRequest> LeaveRequests { get; set; }
}
