using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;
using Microsoft.Extensions.Configuration;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class OrganisationController : ControllerBase
{

    private readonly ServiceAccountsService _serviceAccountService;
    private readonly OrgAccountsService _orgAccountService;

    private readonly WalletsService _walletService;

    private readonly ILogger<SubscribersController> _logger;

    public IConfiguration Configuration;


    public OrganisationController(
        ServiceAccountsService serviceAccountService,
        OrgAccountsService orgAccountService,
        WalletsService walletService,
        ILogger<SubscribersController> logger,
        IConfiguration configuration
    )
    {
        _serviceAccountService = serviceAccountService;
        _orgAccountService = orgAccountService;
        _walletService = walletService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<OrgAccount>> Get() =>
         await _orgAccountService.GetOrgAccounts();

    [HttpGet("{id}")]
    public async Task<ActionResult<OrgAccount>> Get(string id)
    {
        var orgAccount = await _orgAccountService.GetOrgAccount(id);

        if (orgAccount is null)
        {
            _logger.LogInformation("ERROR Finding a Org Account: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return orgAccount;
    }


    [HttpPost]
    public async Task<IActionResult> Post(OrgAccount orgAccount)
    {
        try
        {

            var account = await _orgAccountService.GetOrgAccountbyMSISDN(orgAccount.MSISDN);
            if (account is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Organisation!: {orgAccount} with {MSISDN} {Now}", orgAccount, DateTime.Now, orgAccount.MSISDN);
                return Created( "Please Contact Support",orgAccount);
            }

            var services = Configuration.GetSection("SupportedServicesOrganisation").Get<List<Service>>();
            Wallet wallet = new Wallet()
            { 
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Currency = "KSH",
                LastOperatedAt = DateTime.Now,
            };
            List<Wallet> wallets = new List<Wallet>();
            wallets.Add(wallet);
            ServiceAccount serviceAccount = new ServiceAccount()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                SupportedServices = services,
                Wallets = wallets
            };
            serviceAccount.Wallets[0].ServiceAccountId = serviceAccount.Id;
            List<TransactionLimit> transactionLimits = new List<TransactionLimit>();
            transactionLimits.Add(new TransactionLimit() { Id = Guid.NewGuid().ToString(), LimitName = "Customer Organisation", MaximumValue = 100, Count = 5, Duration = 3600, LimitType = "Velocity" });

            orgAccount.ServiceAccount = serviceAccount;
            orgAccount.ServiceAccountId = serviceAccount.Id;
            orgAccount.Id = Guid.NewGuid().ToString();
            orgAccount.BalanceLimit = new ValueLimit() { Id = Guid.NewGuid().ToString(), LimitName = orgAccount.UserAccountRole, MaximumValue = 600 };
            orgAccount.TransactionLimits = transactionLimits;
            orgAccount.CreatedAt = DateTime.Now;

            await _serviceAccountService.CreateServiceAccount(serviceAccount);
            await _orgAccountService.CreateOrgAccount(orgAccount);

            return CreatedAtAction(nameof(Get), new { id = orgAccount.Id }, orgAccount);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(20, ex, "Error while Adding Customer Organisation - {orgAccount}", orgAccount);
            return Problem("Error while Adding Organisation");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, OrgAccount orgAccount)
    {

        try
        {
            var account = await _orgAccountService.GetOrgAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Customer Ogranisation Account: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }

            orgAccount.Id = account.Id;

            await _orgAccountService.UpdateOrgAccount(id, orgAccount);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(21, ex, "Error while Updating Organisation - {orgAccount}, {id}", orgAccount, id);
            return Problem("Error while Updating Subscriber");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var account = await _orgAccountService.GetOrgAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Customer Organisation: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }

            await _orgAccountService.DeleteOrgAccount(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(23, ex, "Error while Archiving Organisation -  {id}", id);
            return Problem("Error while Removing Organisation");
        }

    }

}