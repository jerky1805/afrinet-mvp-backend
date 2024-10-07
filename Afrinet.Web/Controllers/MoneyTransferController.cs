using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;
using IMT.Models;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class MoneyTransferController : ControllerBase
{

    private readonly MoneyTransferService _moneyTransferService;


    private readonly ILogger<MoneyTransferController> _logger;

    public IConfiguration Configuration;


    public MoneyTransferController(
        MoneyTransferService moneyTransferService,
        ILogger<MoneyTransferController> logger,
        IConfiguration configuration
    )
    {
        _moneyTransferService = moneyTransferService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<MoneyTransferViewModel>> Get() =>
         await _moneyTransferService.GetMoneyTransfers();

    [HttpGet("{id}")]
    public async Task<ActionResult<MoneyTransferViewModel>> Get(string id)
    {
        var moneyTransfer = await _moneyTransferService.GetMoneyTransfer(id);
        if (moneyTransfer is null)
        {
            _logger.LogInformation("ERROR Finding a Loan Details: {id} from {UtcNow}", id, DateTime.UtcNow);
            return NotFound();
        }
        return moneyTransfer;
    }


    [HttpPost]
    public async Task<IActionResult> Post(MoneyTransferViewModel moneyTransferViewModel)
    {
        try
        {

            moneyTransferViewModel.Id = Guid.NewGuid().ToString(); ;
            moneyTransferViewModel.transaction.StartedAt = DateTime.UtcNow;
            moneyTransferViewModel.transaction.LastUpdatedAt = DateTime.UtcNow;
            await _moneyTransferService.CreateMoneyTransfer(moneyTransferViewModel);

            return CreatedAtAction(nameof(Get), new { id = moneyTransferViewModel.Id }, moneyTransferViewModel);
        }
        catch (Exception ex)
        {

            _logger.LogError(410, ex, "Error while Adding Money Transfer Details - {moneyTransferViewModel} ", moneyTransferViewModel);
            return Problem("Error while Adding Money Transfer Details");
        }

    }




}