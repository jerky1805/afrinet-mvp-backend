using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;
using Microsoft.Extensions.Configuration;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SubscribersController : ControllerBase
{

    private readonly ServiceAccountsService _serviceAccountService;
    private readonly UserAccountsService _userAccountService;

    private readonly WalletsService _walletService;

    private readonly ILogger<SubscribersController> _logger;

    public IConfiguration Configuration;


    public SubscribersController(
        ServiceAccountsService serviceAccountService,
        UserAccountsService userAccountService,
        WalletsService walletService,
        ILogger<SubscribersController> logger,
        IConfiguration configuration
    )
    {
        _serviceAccountService = serviceAccountService;
        _userAccountService = userAccountService;
        _walletService = walletService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<UserAccount>> Get() =>
         await _userAccountService.GetUserAccounts();

    [HttpGet("{id}")]
    public async Task<ActionResult<UserAccount>> Get(string id)
    {
        var userAccount = await _userAccountService.GetUserAccount(id);

        if (userAccount is null)
        {
            _logger.LogInformation("ERROR Finding a User Account: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return userAccount;
    }

    [HttpGet("{channel}/{msisdn}")]
    public async Task<ActionResult<UserAccount>> Get(string msisdn, string channel = "web")
    {
        var userAccount = await _userAccountService.GetUserAccountbyMSISDN(msisdn);

        if (userAccount is null)
        {
            _logger.LogInformation("ERROR Finding a User Account: {msisdn} from {Now}", msisdn, DateTime.Now);
            return NotFound();
        }
        return userAccount;
    }
    [HttpPost]
    public async Task<IActionResult> Post(UserAccount userAccount)
    {
        try
        {

            var account = await _userAccountService.GetUserAccountbyMSISDN(userAccount.MSISDN);
            if (account is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Subscriber!: {userAccount} with {MSISDN} {Now}", userAccount, DateTime.Now, userAccount.MSISDN);
                return Created("Please Contact Support", userAccount);
            }

            var services = Configuration.GetSection("SupportedServicesSubscribers").Get<List<Service>>();
            Wallet wallet = new Wallet()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Currency = "KES",
                LastOperatedAt = DateTime.Now,
                WalletType = "MoneyWallet"
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
            transactionLimits.Add(new TransactionLimit() { Id = Guid.NewGuid().ToString(), LimitName = userAccount.UserAccountRole, MaximumValue = 100, Count = 5, Duration = 3600, LimitType = "Velocity" });

            userAccount.ServiceAccount = serviceAccount;
            userAccount.ServiceAccountId = serviceAccount.Id;
            userAccount.Id = Guid.NewGuid().ToString();
            userAccount.BalanceLimit = new ValueLimit() { Id = Guid.NewGuid().ToString(), LimitName = userAccount.UserAccountRole, MaximumValue = 600 };
            userAccount.TransactionLimits = transactionLimits;
            userAccount.CreatedAt = DateTime.Now;
            userAccount.Status = "Created";

            //TODO: Add Infrastructure for KYC 

            await _serviceAccountService.CreateServiceAccount(serviceAccount);
            await _userAccountService.CreateUserAccount(userAccount);
            //TODO: Send Activation Request
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync("http://localhost:5122/api/Activation/",
                new ActivationRequest
                {
                    Id = Guid.NewGuid().ToString(),
                    MSISDN = userAccount.MSISDN,
                    SubscriberId = userAccount.Id,
                    CreatedAt = DateTime.Now,
                    Status = "Initiated"
                }
                    );
            
            }

            return CreatedAtAction(nameof(Get), new { id = userAccount.Id }, userAccount);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(10, ex, "Error while Adding Subscriber - {userAccount}", userAccount);
            return Problem("Error while Adding Subscriber");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UserAccount userAccount)
    {

        try
        {
            var account = await _userAccountService.GetUserAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a User Account: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }

            userAccount.Id = account.Id;

            await _userAccountService.UpdateUserAccount(id, userAccount);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(11, ex, "Error while Updating Subscriber - {userAccount}, {id}", userAccount, id);
            return Problem("Error while Updating Subscriber");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var account = await _userAccountService.GetUserAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a User Account: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }

            await _userAccountService.DeleteUserAccount(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(13, ex, "Error while Archiving Subscriber -  {id}", id);
            return Problem("Error while Removing Subscriber");
        }

    }

}