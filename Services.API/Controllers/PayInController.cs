using Services.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;

namespace Services.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PayInController : ControllerBase
{

    private readonly ILogger<PayInController> _logger;
    private readonly IQueue _queue;

    private readonly TransactionService _transactionService;






    public PayInController(
        ILogger<PayInController> logger,
        IQueue queue,
        TransactionService transactionService
    )
    {
        _logger = logger;
        _queue = queue;
        _transactionService = transactionService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> Get(string id)
    {
        var transaction = await _transactionService.GetTransaction(id);

        if (transaction is null)
        {
            _logger.LogInformation("ERROR Finding a Transaction: {id} from {Now}", id, DateTime.UtcNow);
            return NotFound();
        }
        return transaction;
    }


    [HttpPost]
    public async Task<IActionResult> Post(Transaction transaction)
    {
        if (transaction == null)
            return BadRequest();

        else
        {
            transaction.Id = Guid.NewGuid().ToString();
            transaction.TransactionType = "PayIn";
            transaction.State = "Initiated";
            transaction.StartedAt = DateTime.UtcNow;
            //TODO : Implement Fees 
            //TODO : Implement Taxes
            await _transactionService.CreateTransaction(transaction);

            using (var client = new HttpClient())
            {

                var initiatingAccount = await client.GetFromJsonAsync<AgentAccount>(new Uri("http://localhost:5032/api/Agent/" + transaction.InitatingAccountID));
                if (initiatingAccount == null)
                {
                    _logger.LogInformation("Initiating Account Does Not Exist: User Account with {InitatingAccountID} {UtcNow}", transaction.InitatingAccountID, DateTime.UtcNow);
                    return NotFound("Contact Customer Support");
                }
                var recipientAccount = await client.GetFromJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + transaction.ReceivingAccountID));
                if (recipientAccount == null)
                {
                    _logger.LogInformation("Recipient Account Does Not Exist: User Account with {ReceivingAccountID} {UtcNow}", transaction.ReceivingAccountID, DateTime.UtcNow);
                    return NotFound("Contact Customer Support");
                }


                //TODO : Implement Limit Checks
                initiatingAccount.ServiceAccount.Wallets[0].Balance -= transaction.Amount;
                recipientAccount.ServiceAccount.Wallets[0].Balance += transaction.Amount;
                await client.PutAsJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + transaction.ReceivingAccountID), recipientAccount);
                await client.PutAsJsonAsync<AgentAccount>(new Uri("http://localhost:5032/api/Agent/" + transaction.InitatingAccountID), initiatingAccount);
                transaction.State = "Completed";
                transaction.CompletedAt = DateTime.UtcNow;
                await _transactionService.UpdateTransaction(transaction.Id, transaction);
                return CreatedAtAction(nameof(Get), new { id = transaction.Id }, transaction);
            }

        }

    }

}