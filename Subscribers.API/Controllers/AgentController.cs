using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;
using Microsoft.Extensions.Configuration;

namespace Subscribers.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AgentController : ControllerBase
{

    private readonly ServiceAccountsService _serviceAccountService;
    private readonly AgentAccountsService _agentAccountService;

    private readonly WalletsService _walletService;

    private readonly ILogger<SubscribersController> _logger;

    public IConfiguration Configuration;


    public AgentController(
        ServiceAccountsService serviceAccountService,
        AgentAccountsService agentAccountsService,
        WalletsService walletService,
        ILogger<SubscribersController> logger,
        IConfiguration configuration
    )
    {
        _serviceAccountService = serviceAccountService;
        _agentAccountService = agentAccountsService;
        _walletService = walletService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<AgentAccount>> Get() =>
         await _agentAccountService.GetAgentAccounts();

    [HttpGet("{id}")]
    public async Task<ActionResult<AgentAccount>> Get(string id)
    {
        var agentAccount = await _agentAccountService.GetAgentAccount(id);

        if (agentAccount is null)
        {
            _logger.LogInformation("ERROR Finding a Agent Account: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        }
        return agentAccount;
    }


    [HttpPost]
    public async Task<IActionResult> Post(AgentAccount agentAccount)
    {
        try
        {

            var account = await _agentAccountService.GetAgentAccountbyMSISDN(agentAccount.MSISDN);
            if (account is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Agent!: {agentAccount} with {MSISDN} {Now}", agentAccount, DateTime.Now, agentAccount.MSISDN);
                return Created( "Please Contact Support",agentAccount);
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
            transactionLimits.Add(new TransactionLimit() { Id = Guid.NewGuid().ToString(), LimitName = agentAccount.AccountType + " Transaction Limit", MaximumValue = 100, Count = 5, Duration = 3600, LimitType = "Velocity" });

            agentAccount.ServiceAccount = serviceAccount;
            agentAccount.ServiceAccountId = serviceAccount.Id;
            agentAccount.Id = Guid.NewGuid().ToString();
            agentAccount.BalanceLimit = new ValueLimit() { Id = Guid.NewGuid().ToString(), LimitName = agentAccount.AccountType + " Balance Limit", MaximumValue = 600 };
            agentAccount.TransactionLimits = transactionLimits;
            agentAccount.CreatedAt = DateTime.Now;
            agentAccount.Status = "Created";

//TODO: Add Infrastructure for KYC 

            await _serviceAccountService.CreateServiceAccount(serviceAccount);
            await _agentAccountService.CreateAgentAccount(agentAccount);

            return CreatedAtAction(nameof(Get), new { id = agentAccount.Id }, agentAccount);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(50, ex, "Error while Adding Agent - {agentAccount}", agentAccount);
            return Problem("Error while Adding Agent");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, AgentAccount agentAccount)
    {

        try
        {
            var account = await _agentAccountService.GetAgentAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Agent Account: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }

            agentAccount.Id = account.Id;

            await _agentAccountService.UpdateAgentAccount(id, agentAccount);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(41, ex, "Error while Updating Agent - {agentAccount}, {id}", agentAccount, id);
            return Problem("Error while Updating Subscriber");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var account = await _agentAccountService.GetAgentAccount(id);

            if (account is null)
            {
                _logger.LogInformation("ERROR Finding a Agent Account: {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }

            await _agentAccountService.DeleteAgentAccount(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(43, ex, "Error while Archiving Agent -  {id}", id);
            return Problem("Error while Removing Agent");
        }

    }

}