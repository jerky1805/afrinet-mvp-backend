using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using RAI.Models;
using RAI.Services;

namespace RAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AgencyAdminController : ControllerBase
{

    private readonly AgencyAdminService _agencyAdminService;


    private readonly ILogger<AgencyAdminController> _logger;

    public IConfiguration Configuration;


    public AgencyAdminController(
        AgencyAdminService agencyAdminService,
        ILogger<AgencyAdminController> logger,
        IConfiguration configuration
    )
    {
        _agencyAdminService = agencyAdminService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<AgencyAdmin>> Get() =>
         await _agencyAdminService.GetAgencyAdmins();

    [HttpGet("{id}")]
    public async Task<ActionResult<AgencyAdmin>> Get(string id)
    {
        var agencyAdmin = await _agencyAdminService.GetAgencyAdmin(id);

        if (agencyAdmin is null)
        {
            _logger.LogInformation("ERROR Finding a Head Office Account: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return agencyAdmin;
    }


    [HttpPost]
    public async Task<IActionResult> Post(AgencyAdmin agencyAdmin)
    {
        try
        {
            agencyAdmin.Id = Guid.NewGuid().ToString();
            agencyAdmin.CreatedAt = DateTime.Now;

            await _agencyAdminService.CreateAgencyAdmin(agencyAdmin);

            return CreatedAtAction(nameof(Get), new { id = agencyAdmin.Id }, agencyAdmin);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(240, ex, "Error while Adding Agency Admin - {agencyAdmin}", agencyAdmin);
            return Problem("Error while Adding Agency Admin");
        }

    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, AgencyAdmin agencyAdmin)
    {

        try
        {
            var a = await _agencyAdminService.GetAgencyAdmin(id);

            if (a is null)
            {
                _logger.LogInformation("ERROR Finding a Agency Admint: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            agencyAdmin.UpdatedAt = DateTime.UtcNow;
            await _agencyAdminService.UpdateAgencyAdmin(id, agencyAdmin);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(241, ex, "Error while Updating Agency Admin - {agencyAdmin}, {id}", agencyAdmin, id);
            return Problem("Error while Updating Agency Admin");
        }

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var s = await _agencyAdminService.GetAgencyAdmin(id);

            if (s is null)
            {
                _logger.LogInformation("ERROR Finding a Agency Admin: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _agencyAdminService.DeleteAgencyAdmin(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(243, ex, "Error while Archiving Agency Admin -  {id}", id);
            return Problem("Error while Removing Agency Admin");
        }

    }

}