
using Microsoft.AspNetCore.Mvc;
using RAI.Models;
using RAI.Services;

namespace RAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AgencyController : ControllerBase
{

    private readonly AgencyService _agencyService;


    private readonly ILogger<AgencyController> _logger;

    public IConfiguration Configuration;


    public AgencyController(
        AgencyService agencyService,
        ILogger<AgencyController> logger,
        IConfiguration configuration
    )
    {
        _agencyService = agencyService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<Agency>> Get() =>
         await _agencyService.GetAgencies();

    [HttpGet("{id}")]
    public async Task<ActionResult<Agency>> Get(string id)
    {
        var a = await _agencyService.GetAgency(id);

        if (a is null)
        {
            _logger.LogInformation("ERROR Finding a Agency: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return a;
    }


    [HttpPost]
    public async Task<IActionResult> Post(Agency agency)
    {
        try
        {

            var a = await _agencyService.GetAgency(agency.Id);
            if (a is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Agency !: {agency} with {Id} {UtcNow}", agency, DateTime.UtcNow, agency.Id);
                return Created("Please Contact Support", agency);
            }


            agency.Id = Guid.NewGuid().ToString();
            agency.CreatedAt = DateTime.UtcNow;

            await _agencyService.CreateAgency(agency);

            return CreatedAtAction(nameof(Get), new { id = agency.Id }, agency);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(230, ex, "Error while Adding Agency - {agency}", agency);
            return Problem("Error while Adding Agency");
        }

    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Agency agency)
    {

        try
        {
            var a = await _agencyService.GetAgency(id);

            if (a is null)
            {
                _logger.LogInformation("ERROR Finding a Agency: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }

            agency.UpdatedAt = DateTime.UtcNow;
            await _agencyService.UpdateAgency(id, agency);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(231, ex, "Error while Updating Agency - {agency}, {id}", agency, id);
            return Problem("Error while Updating Agency");
        }

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var s = await _agencyService.GetAgency(id);

            if (s is null)
            {
                _logger.LogInformation("ERROR Finding a Agency: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _agencyService.DeleteAgency(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(233, ex, "Error while Archiving Agency -  {id}", id);
            return Problem("Error while Removing Agency");
        }

    }

}