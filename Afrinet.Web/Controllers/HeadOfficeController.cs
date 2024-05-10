using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class HeadOfficeController : ControllerBase
{

    private readonly ServiceAccountsService _serviceAccountService;
    private readonly HeadOfficeAccountsService _headOfficeAccountService;

    private readonly WalletsService _walletService;

    private readonly ILogger<HeadOfficeController> _logger;

    public IConfiguration Configuration;


    public HeadOfficeController(
        ServiceAccountsService serviceAccountService,
        HeadOfficeAccountsService headOfficeAccountsService,
        WalletsService walletService,
        ILogger<HeadOfficeController> logger,
        IConfiguration configuration
    )
    {
        _serviceAccountService = serviceAccountService;
        _headOfficeAccountService = headOfficeAccountsService;
        _walletService = walletService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<HeadOffice>> Get() =>
         await _headOfficeAccountService.GetHeadOfficeAccounts();

    [HttpGet("{id}")]
    public async Task<ActionResult<HeadOffice>> Get(string id)
    {
        var headOfficeAccount = await _headOfficeAccountService.GetHeadOfficeAccount(id);

        if (headOfficeAccount is null)
        {
            _logger.LogInformation("ERROR Finding a Head Office Account: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return headOfficeAccount;
    }


    [HttpPost]
    public async Task<IActionResult> Post(HeadOffice headOfficeAccount)
    {
        try
        {

            var account = await _headOfficeAccountService.GetHeadOfficeAccountbyMSISDN(headOfficeAccount.MSISDN);
            if (account is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Head Office!: {headOfficeAccount} with {MSISDN} {Now}", headOfficeAccount, DateTime.Now, headOfficeAccount.MSISDN);
                return Created("Please Contact Support", headOfficeAccount);
            }

            var services = Configuration.GetSection("SupportedServicesAgents").Get<List<Service>>();
            Wallet wallet = new Wallet()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Currency = "KES",
                LastOperatedAt = DateTime.Now,
            };
            Wallet wallet1 = new Wallet()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Currency = "KES",
                LastOperatedAt = DateTime.Now,
                WalletType = "Working Wallet"
            };
            Wallet wallet2 = new Wallet()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Currency = "KES",
                LastOperatedAt = DateTime.Now,
                WalletType = "Control Wallet"
            };
            List<Wallet> wallets = new List<Wallet>();
            wallets.Add(wallet);
            wallets.Add(wallet1);
            wallets.Add(wallet2);
            ServiceAccount serviceAccount = new ServiceAccount()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                SupportedServices = services,
                Wallets = wallets
            };
            serviceAccount.Wallets[0].ServiceAccountId = serviceAccount.Id;
            List<TransactionLimit> transactionLimits = new List<TransactionLimit>();
            transactionLimits.Add(new TransactionLimit() { Id = Guid.NewGuid().ToString(), LimitName = "Head Office Transaction Limit", MaximumValue = 100, Count = 5, Duration = 3600, LimitType = "Velocity" });

            headOfficeAccount.ServiceAccount = serviceAccount;
            headOfficeAccount.ServiceAccountId = serviceAccount.Id;
            headOfficeAccount.Id = Guid.NewGuid().ToString();
            headOfficeAccount.BalanceLimit = new ValueLimit() { Id = Guid.NewGuid().ToString(), LimitName = "Head Office Balance Limit", MaximumValue = 600 };
            headOfficeAccount.TransactionLimits = transactionLimits;
            headOfficeAccount.CreatedAt = DateTime.Now;
            headOfficeAccount.Status = "Created";

            await _serviceAccountService.CreateServiceAccount(serviceAccount);
            await _headOfficeAccountService.CreateHeadOfficeAccount(headOfficeAccount);

            return CreatedAtAction(nameof(Get), new { id = headOfficeAccount.Id }, headOfficeAccount);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(30, ex, "Error while Adding Head Office Organisation - {headOfficeAccount}", headOfficeAccount);
            return Problem("Error while Adding Head Office");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, HeadOffice headOfficeAccount)
    {

        try
        {
            var account = await _headOfficeAccountService.GetHeadOfficeAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Head Office: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }

            headOfficeAccount.Id = headOfficeAccount.Id;

            await _headOfficeAccountService.UpdateHeadOfficeAccount(id, headOfficeAccount);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(31, ex, "Error while Updating Head Office - {headOfficeAccount}, {id}", headOfficeAccount, id);
            return Problem("Error while Updating Head Office");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var account = await _headOfficeAccountService.GetHeadOfficeAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Head Office: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _headOfficeAccountService.DeleteHeadOfficeAccount(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(33, ex, "Error while Archiving Head Office -  {id}", id);
            return Problem("Error while Removing Head Office");
        }

    }

}