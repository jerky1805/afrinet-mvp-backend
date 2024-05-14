using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using RAI.Models;
using RAI.Services;

namespace RAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StaffController : ControllerBase
{

    private readonly StaffService _staffService;


    private readonly ILogger<StaffController> _logger;

    public IConfiguration Configuration;


    public StaffController(
        StaffService staffService,
        ILogger<StaffController> logger,
        IConfiguration configuration
    )
    {
        _staffService = staffService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<Staff>> Get() =>
         await _staffService.GetStaffs();

    [HttpGet("{id}")]
    public async Task<ActionResult<Staff>> Get(string id)
    {
        var staff = await _staffService.GetStaff(id);

        if (staff is null)
        {
            _logger.LogInformation("ERROR Finding a Head Office Account: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return staff;
    }


    [HttpPost]
    public async Task<IActionResult> Post(Staff staff)
    {
        try
        {

            var s = await _staffService.GetStaff(staff.Id);
            if (s is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Staff !: {staff} with {Id} {UtcNow}", staff, DateTime.UtcNow, staff.Id);
                return Created("Please Contact Support", staff);
            }


            staff.Id = Guid.NewGuid().ToString();
            staff.CreatedAt = DateTime.Now;

            await _staffService.CreateStaff(staff);

            return CreatedAtAction(nameof(Get), new { id = staff.Id }, staff);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(220, ex, "Error while Adding Staff - {staff}", staff);
            return Problem("Error while Adding Staff");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Staff staff)
    {

        try
        {
            var s = await _staffService.GetStaff(id);

            if (s is null)
            {
                _logger.LogInformation("ERROR Finding a Staff: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }



            await _staffService.UpdateStaff(id, staff);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(221, ex, "Error while Updating staff - {Staff}, {id}", staff, id);
            return Problem("Error while Updating Staff");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var s = await _staffService.GetStaff(id);

            if (s is null)
            {
                _logger.LogInformation("ERROR Finding a Staff: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _staffService.DeleteStaff(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(223, ex, "Error while Archiving Staff -  {id}", id);
            return Problem("Error while Removing Staff");
        }

    }

}