using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CompanyDetailsController : ControllerBase
{

    private readonly CompanyDetailsService _companyDetailsService;


    private readonly ILogger<CompanyDetailsController> _logger;

    public IConfiguration Configuration;


    public CompanyDetailsController(
        CompanyDetailsService companyDetailsService,
        ILogger<CompanyDetailsController> logger,
        IConfiguration configuration
    )
    {
        _companyDetailsService = companyDetailsService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<CompanyDetails>> Get() =>
         await _companyDetailsService.GetCompanyDetails();

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDetails>> Get(string id)
    {
        var companyDetails = await _companyDetailsService.GetCompanyDetail(id);
        if (companyDetails is null)
        {
            _logger.LogInformation("ERROR Finding a Personal Details: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return companyDetails;
    }


    [HttpPost]
    public async Task<IActionResult> Post(CompanyDetails companyDetails)
    {
        try
        {

            //var person = await _personalDetailsService.GetPersonalDetail(personalDetails.Id);
            if (companyDetails.Id is null)
            {
                _logger.LogInformation("Attempt to Create a Company without related  Personal Detail!: {companyDetails} on {UtcNow}", companyDetails, DateTime.UtcNow);
                return Created("Please Contact Support", companyDetails);
            }

            //personalDetails.Id = Guid.NewGuid().ToString(); ;
            await _companyDetailsService.CreateCompanyDetail(companyDetails);

            return CreatedAtAction(nameof(Get), new { id = companyDetails.Id }, companyDetails);
        }
        catch (Exception ex)
        {

            _logger.LogError(80, ex, "Error while Adding Company Details - {companyDetails} ", companyDetails);
            return Problem("Error while Adding Company Details");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CompanyDetails companyDetails)
    {

        try
        {
            var company = await _companyDetailsService.GetCompanyDetail(id);

            if (company is null)
            {
                _logger.LogInformation("ERROR Finding a Company Details: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }

            companyDetails.Id = id;

            await _companyDetailsService.UpdateCompanyDetail(id, companyDetails);

            return NoContent();
        }
        catch (Exception ex)
        {

            _logger.LogError(81, ex, "Error while Updating Company Details - {companyDetails}, {id}", companyDetails, id);
            return Problem("Error while Updating Personal Details");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var company = await _companyDetailsService.GetCompanyDetail(id);

            if (company is null)
            {
                _logger.LogInformation("ERROR Finding a Company Details: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _companyDetailsService.DeleteCompanyDetail(id);
            return NoContent();
        }
        catch (Exception ex)
        {

            _logger.LogError(83, ex, "Error while Deleting Company Details -  {id} \n {UtcNow}", id, DateTime.UtcNow);
            return Problem("Error while Removing Company Details");
        }

    }

}