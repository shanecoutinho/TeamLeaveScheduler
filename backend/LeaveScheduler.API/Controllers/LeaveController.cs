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
[HttpPut("{id}/reject")]
public async Task<IActionResult> RejectLeaveRequest(
    int id,
    RejectLeaveRequestDto request)
{
    try
    {
        var leaveRequest = await _leaveService.RejectLeaveRequest(
            id,
            request.RejectionReason);

        return Ok(leaveRequest);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}

[HttpGet]
public async Task<IActionResult> GetAllLeaveRequests()
{
    var requests = await _leaveService.GetAllLeaveRequests();

    return Ok(requests);
}   

[HttpGet("{id}")]
public async Task<IActionResult> GetLeaveRequestById(int id)
{
    var request = await _leaveService.GetLeaveRequestById(id);

    if (request == null)
    {
        return NotFound();
    }

    return Ok(request);
}
}