using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LoanDetailsController : ControllerBase
{

    private readonly LoanDetailsService _loanDetailsService;


    private readonly ILogger<LoanDetailsController> _logger;

    public IConfiguration Configuration;


    public LoanDetailsController(
        LoanDetailsService loanDetailsService,
        ILogger<LoanDetailsController> logger,
        IConfiguration configuration
    )
    {
        _loanDetailsService = loanDetailsService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<LoanDetails>> Get() =>
         await _loanDetailsService.GetLoanDetails();

    [HttpGet("{id}")]
    public async Task<ActionResult<LoanDetails>> Get(string id)
    {
        var loanDetails = await _loanDetailsService.GetLoanDetail(id);
        if (loanDetails is null)
        {
            _logger.LogInformation("ERROR Finding a Loan Details: {id} from {UtcNow}", id, DateTime.UtcNow);
            return NotFound();
        }
        return loanDetails;
    }


    [HttpPost]
    public async Task<IActionResult> Post(LoanDetails loanDetails)
    {
        try
        {

            //var person = await _personalDetailsService.GetPersonalDetail(personalDetails.Id);
            // if (loanDetails.Id is null)
            // {
            //     _logger.LogInformation("Attempt to Create a Loan without related  Personal and Company Detail!: {loanDetails} on {UtcNow}", loanDetails, DateTime.UtcNow);
            //     return Created("Please Contact Support", loanDetails);
            // }

            loanDetails.Id = Guid.NewGuid().ToString(); ;
            loanDetails.CreatedAt = DateTime.UtcNow;
            loanDetails.UpdatedAt = DateTime.UtcNow;
            await _loanDetailsService.CreateLoanDetail(loanDetails);

            return CreatedAtAction(nameof(Get), new { id = loanDetails.Id }, loanDetails);
        }
        catch (Exception ex)
        {

            _logger.LogError(90, ex, "Error while Adding Loan Details - {loanDetails} ", loanDetails);
            return Problem("Error while Adding Loan Details");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, LoanDetails loanDetails)
    {

        try
        {
            var loan = await _loanDetailsService.GetLoanDetail(id);

            if (loan is null)
            {
                _logger.LogInformation("ERROR Finding a Loan Details: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }

            loanDetails.Id = id;

            await _loanDetailsService.UpdateLoanDetail(id, loanDetails);

            return NoContent();
        }
        catch (Exception ex)
        {

            _logger.LogError(91, ex, "Error while Updating Loan Details - {loanDetails}, {id}", loanDetails, id);
            return Problem("Error while Updating Personal Details");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var loan = await _loanDetailsService.GetLoanDetail(id);

            if (loan is null)
            {
                _logger.LogInformation("ERROR Finding a Loan Details: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _loanDetailsService.DeleteLoanDetail(id);
            return NoContent();
        }
        catch (Exception ex)
        {

            _logger.LogError(93, ex, "Error while Deleting Loan Details -  {id} \n {UtcNow}", id, DateTime.UtcNow);
            return Problem("Error while Removing Loan Details");
        }

    }

}