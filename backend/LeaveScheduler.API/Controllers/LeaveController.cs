using LeaveScheduler.API.DTOs;
using LeaveScheduler.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeaveScheduler.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private readonly LeaveService _leaveService;

    public LeaveController(LeaveService leaveService)
    {
        _leaveService = leaveService;
    }
}