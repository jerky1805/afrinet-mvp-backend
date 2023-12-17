using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;
using Microsoft.Extensions.Configuration;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class BranchController : ControllerBase
{

    private readonly ServiceAccountsService _serviceAccountService;
    private readonly BranchAccountsService _branchAccountService;

    private readonly WalletsService _walletService;

    private readonly ILogger<SubscribersController> _logger;

    public IConfiguration Configuration;


    public BranchController(
        ServiceAccountsService serviceAccountService,
        BranchAccountsService branchAccountService,
        WalletsService walletService,
        ILogger<SubscribersController> logger,
        IConfiguration configuration
    )
    {
        _serviceAccountService = serviceAccountService;
        _branchAccountService = branchAccountService;
        _walletService = walletService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet("{headOfficeId}")]
    public async Task<List<Branch>> Get(string headOfficeId) =>
         await _branchAccountService.GetBranchAccountsbyHeadOfficeId(headOfficeId);

    [HttpGet("{headOfficeId}/{id}")]
    public async Task<ActionResult<Branch>> Get(string headOfficeId, string id)
    {
        var branchAccount = await _branchAccountService.GetBranchAccount(id);

        if (branchAccount is null)
        {
            _logger.LogInformation("ERROR Finding a Branch Account: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        if (branchAccount.HeadOfficeId != headOfficeId)
        {
            _logger.LogInformation("ERROR Head Office does not Match Branch Account's: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return branchAccount;
    }

    // [HttpGet]
    // public async Task<ActionResult<Branch>> GetforHeadOffice(string id)
    // {
    //     var branchAccount = await _branchAccountService.GetBranchAccount(id);

    //     if (branchAccount is null)
    //     {
    //         _logger.LogInformation("ERROR Finding a Branch Account: {id} from {Now}", id, DateTime.Now);
    //         return NotFound();
    //     }
    //     return branchAccount;
    // }


    [HttpPost]
    public async Task<IActionResult> Post(Branch branchAccount)
    {
        try
        {

            var account = await _branchAccountService.GetBranchAccount(branchAccount.MSISDN);
            if (account is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Branch!: {branchAccount} with {MSISDN} {Now}", branchAccount, DateTime.Now, branchAccount.MSISDN);
                return Created("Please Contact Support", branchAccount);
            }

            var services = Configuration.GetSection("SupportedServicesAgents").Get<List<Service>>();
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
            transactionLimits.Add(new TransactionLimit() { Id = Guid.NewGuid().ToString(), LimitName = "Branch Transaction Limit", MaximumValue = 100, Count = 5, Duration = 3600, LimitType = "Velocity" });

            branchAccount.ServiceAccount = serviceAccount;
            branchAccount.ServiceAccountId = serviceAccount.Id;
            branchAccount.Id = Guid.NewGuid().ToString();
            branchAccount.BalanceLimit = new ValueLimit() { Id = Guid.NewGuid().ToString(), LimitName = "Branch Balance Limit", MaximumValue = 600 };
            branchAccount.TransactionLimits = transactionLimits;
            branchAccount.CreatedAt = DateTime.Now;
            branchAccount.Status = "Created";

            await _serviceAccountService.CreateServiceAccount(serviceAccount);
            await _branchAccountService.CreateBranchAccount(branchAccount);

            return Ok(await _branchAccountService.GetBranchAccount(branchAccount.Id));
        }
        catch (System.Exception ex)
        {

            _logger.LogError(40, ex, "Error while Adding Branch  - {branchAccount}", branchAccount);
            return Problem("Error while Adding Branch");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Branch branchAccount)
    {

        try
        {
            var account = await _branchAccountService.GetBranchAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Branch Account: {id} from {Now}", id, DateTime.Now);
                Console.WriteLine(" Branch Account: {id}",branchAccount);
                return NotFound();
            }

            branchAccount.Id = account.Id;

            await _branchAccountService.UpdateBranchAccount(id, branchAccount);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(41, ex, "Error while Updating Branch - {branchAccount}, {id}", branchAccount, id);
            return Problem("Error while Updating Branch");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var account = await _branchAccountService.GetBranchAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Branch: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }
            //TODO Check that Agents belonging to Branch have been archived. 
            await _branchAccountService.DeleteBranchAccount(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(43, ex, "Error while Archiving Branch -  {id}", id);
            return Problem("Error while Removing Branch");
        }

    }

}