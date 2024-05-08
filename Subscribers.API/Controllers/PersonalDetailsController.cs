using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PersonalDetailsController : ControllerBase
{

    private readonly PersonalDetailsService _personalDetailsService;


    private readonly ILogger<PersonalDetailsController> _logger;

    public IConfiguration Configuration;


    public PersonalDetailsController(
        PersonalDetailsService personalDetailsService,
        ILogger<PersonalDetailsController> logger,
        IConfiguration configuration
    )
    {
        _personalDetailsService = personalDetailsService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<PersonalDetails>> Get() =>
         await _personalDetailsService.GetPersonalDetails();

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonalDetails>> Get(string id)
    {
        var personalDetails = await _personalDetailsService.GetPersonalDetail(id);
        if (personalDetails is null)
        {
            _logger.LogInformation("ERROR Finding a Personal Details: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return personalDetails;
    }


    [HttpPost]
    public async Task<IActionResult> Post(PersonalDetails personalDetails)
    {
        try
        {

            //var person = await _personalDetailsService.GetPersonalDetail(personalDetails.Id);
            if (personalDetails.Id is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Personal Detail!: {personalDetails} on {UtcNow}", personalDetails, DateTime.UtcNow);
                return Created("Please Contact Support", personalDetails);
            }

            personalDetails.Id = Guid.NewGuid().ToString(); ;
            await _personalDetailsService.CreatePersonalDetail(personalDetails);

            return CreatedAtAction(nameof(Get), new { id = personalDetails.Id }, personalDetails);
        }
        catch (Exception ex)
        {

            _logger.LogError(70, ex, "Error while Adding Personal Details - {personalDetails} ", personalDetails);
            return Problem("Error while Adding Personal Details");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, PersonalDetails personalDetails)
    {

        try
        {
            var person = await _personalDetailsService.GetPersonalDetail(id);

            if (person is null)
            {
                _logger.LogInformation("ERROR Finding a Personal Details: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }

            personalDetails.Id = id;

            await _personalDetailsService.UpdatePersonalDetail(id, personalDetails);

            return NoContent();
        }
        catch (Exception ex)
        {

            _logger.LogError(71, ex, "Error while Updating Personal Details - {personalDetails}, {id}", personalDetails, id);
            return Problem("Error while Updating Personal Details");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var account = await _personalDetailsService.GetPersonalDetail(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Personal Details: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _personalDetailsService.DeletePersonalDetail(id);
            return NoContent();
        }
        catch (Exception ex)
        {

            _logger.LogError(73, ex, "Error while Deleting Personal Details -  {id} \n {UtcNow}", id, DateTime.UtcNow);
            return Problem("Error while Removing Personal Details");
        }

    }

}