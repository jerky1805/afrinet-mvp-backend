using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

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
                return Created("Please Contact Support", orgAccount);
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
            orgAccount.AccountType = "Merchant";
            orgAccount.BalanceLimit = new ValueLimit() { Id = Guid.NewGuid().ToString(), LimitName = orgAccount.AccountType, MaximumValue = 600 };
            orgAccount.TransactionLimits = transactionLimits;
            orgAccount.CreatedAt = DateTime.Now;
            orgAccount.Status = "Created";

            // await _serviceAccountService.CreateServiceAccount(serviceAccount);
            await _orgAccountService.CreateOrgAccount(orgAccount);


            using (var client = new HttpClient())
            {
                var accessToken = Configuration.GetSection("ManagementAPIToken").Get<string>();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                List<UserCredential> userCredentials = await client.GetFromJsonAsync<List<UserCredential>>("https://dev-4xt1a0ul.eu.auth0.com/api/v2/users-by-email?email=" + orgAccount.MainContactEmail);
                if (userCredentials.Count > 0)
                {
                    _logger.LogInformation("Attempt to Create a Duplicate UserCredential!: {userCredentials} with {MSISDN} {Now}", userCredentials, DateTime.Now, orgAccount.MSISDN);
                    await _orgAccountService.DeleteOrgAccount(orgAccount.Id);
                    return Problem("Error with Creating Organisation Contact");
                }
                NewUserCredential uc = new NewUserCredential { email = orgAccount.MainContactEmail, connection = "Employers", email_verified = true, 
                family_name = orgAccount.MainContactName, given_name = orgAccount.MainContactOtherNames, password = "plasmolysis2310.wild",
                  name = orgAccount.MainContactName + " " + orgAccount.MainContactOtherNames, 
                 nickname = orgAccount.MainContactEmail,   verify_email = false };
                string payload = JsonSerializer.Serialize(uc);
                var response = await client.PostAsJsonAsync("https://dev-4xt1a0ul.eu.auth0.com/api/v2/users", uc);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("User Created Successfully!: {email} with {MainContactMobileNumber} {Now}", uc.email, DateTime.Now, orgAccount.MainContactMobileNumber);
                    userCredentials = await client.GetFromJsonAsync<List<UserCredential>>("https://dev-4xt1a0ul.eu.auth0.com/api/v2/users-by-email?email=" + orgAccount.MainContactEmail);
                    orgAccount.WebLoginID  = userCredentials[0].user_id;
                    await _orgAccountService.UpdateOrgAccount(orgAccount.Id, orgAccount);
                }
                else
                {
                    string contentDetails = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Problem with Creating Credential for Organisation!: {uc} with {MSISDN} {Now} {contentDetails}", payload, DateTime.Now, orgAccount.MSISDN, contentDetails);
                    await _orgAccountService.DeleteOrgAccount(orgAccount.Id);
                    return Problem("Error with Creating Organisation Contact");
                }
            };


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