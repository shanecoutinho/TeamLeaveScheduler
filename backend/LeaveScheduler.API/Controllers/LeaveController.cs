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

    [HttpPost]
    public async Task<IActionResult> SubmitLeaveRequest(CreateLeaveRequestDto request)
    {
        try
        {
            var leaveRequest = await _leaveService.SubmitLeaveRequest(request);

            return Ok(leaveRequest);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("{id}/approve")]
public async Task<IActionResult> ApproveLeaveRequest(int id)
{
    try
    {
        var leaveRequest = await _leaveService.ApproveLeaveRequest(id);

        return Ok(leaveRequest);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
}