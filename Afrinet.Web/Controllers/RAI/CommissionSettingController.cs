
using Microsoft.AspNetCore.Mvc;
using RAI.Models;
using RAI.Services;

namespace RAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CommissionSettingController : ControllerBase
{

    private readonly CommissionSettingService _commissionSettingService;

    private readonly ILogger<CommissionSettingController> _logger;

    public IConfiguration Configuration;


    public CommissionSettingController(
        CommissionSettingService commissionSettingService,
        ILogger<CommissionSettingController> logger,
        IConfiguration configuration
    )
    {
        _commissionSettingService = commissionSettingService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<CommissionSetting>> Get() =>
         await _commissionSettingService.GetCommissionSettings();

    [HttpGet("{id}")]
    public async Task<ActionResult<CommissionSetting>> Get(string id)
    {
        var commissionSetting = await _commissionSettingService.GetCommissionSetting(id);

        if (commissionSetting is null)
        {
            _logger.LogInformation("ERROR Finding a Commission Setting: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return commissionSetting;
    }


    [HttpPost]
    public async Task<IActionResult> Post(CommissionSetting commissionSetting)
    {
        try
        {

            var c = await _commissionSettingService.GetCommissionSetting(commissionSetting.Id);
            if (c is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Commission Setting !: {commissionSetting} with {Id} {UtcNow}", commissionSetting, DateTime.UtcNow, commissionSetting.Id);
                return Created("Please Contact Support", commissionSetting);
            }


            commissionSetting.Id = Guid.NewGuid().ToString();
            commissionSetting.CreatedAt = DateTime.UtcNow;

            await _commissionSettingService.CreateCommissionSetting(commissionSetting);

            return CreatedAtAction(nameof(Get), new { id = commissionSetting.Id }, commissionSetting);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(260, ex, "Error while Adding Commission Setting - {commissionSetting}", commissionSetting);
            return Problem("Error while Adding Commission Setting");
        }

    }





    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, CommissionSetting commissionSetting)
    {

        try
        {
            var c = await _commissionSettingService.GetCommissionSetting(id);

            if (c is null)
            {
                _logger.LogInformation("ERROR Finding a Commission Setting: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }

            await _commissionSettingService.UpdateCommissionSetting(id, commissionSetting);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(261, ex, "Error while Updating Commission Setting - {CommissionSetting}, {id}", commissionSetting, id);
            return Problem("Error while Updating Commission Setting");
        }

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var c = await _commissionSettingService.GetCommissionSetting(id);

            if (c is null)
            {
                _logger.LogInformation("ERROR Finding a Commission Setting: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _commissionSettingService.DeleteCommissionSetting(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(263, ex, "Error while Archiving Commission Setting -  {id}", id);
            return Problem("Error while Removing Commission Setting");
        }

    }

}