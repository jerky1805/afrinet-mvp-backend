using Services.API.Models;
using Services.API.Services;
using Microsoft.AspNetCore.Mvc;
using Services.API.Service;
using Afrinet.API.Contracts;
using System.Text.Json;
using Afrinet.Models;

namespace Services.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SendMoneyController : ControllerBase
{

    private readonly ILogger<SendMoneyController> _logger;
    private readonly IQueue _queue;

    private readonly TransactionService _transactionService;






    public SendMoneyController(
        ILogger<SendMoneyController> logger,
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
            transaction.TransactionType = "SendMoney";
            transaction.State = "Initiated";
            transaction.StartedAt = DateTime.UtcNow;
            await _transactionService.CreateTransaction(transaction);

            //TODO : Implement Fees 
            //TODO : Implement Taxes
            if (transaction.ReceivingMSISDN == null && transaction.ReceivingAccountID == null)
            {
                _logger.LogInformation("Recipient MSISDN Was Not Supplied  {UtcNow}", DateTime.UtcNow);
                return BadRequest("Recipient MSISDN or Account ID must be supplied");
            }
            using (var client = new HttpClient())
            {
                UserAccount recipientAccount = null;
                if (transaction.ReceivingAccountID != null)
                {
                    recipientAccount = await client.GetFromJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + transaction.ReceivingAccountID));
                    if (recipientAccount == null)
                    {
                        _logger.LogInformation("Recipient Account Does Not Exist: User Account with {ReceivingAccountID} {UtcNow}", transaction.ReceivingAccountID, DateTime.UtcNow);
                        return NotFound("Contact Customer Support");
                    }
                }
                recipientAccount = await client.GetFromJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/web/" + transaction.ReceivingMSISDN));
                if (recipientAccount != null)
                {
                    _logger.LogInformation("Recipient Account Exists with the Supplied MSISDN : User Account with {ReceivingMSISDN} {UtcNow}", transaction.ReceivingMSISDN, DateTime.UtcNow);
                    return NotFound("Contact Customer Support");
                }

                var initiatingAccount = await client.GetFromJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + transaction.InitatingAccountID));
                if (initiatingAccount == null)
                {
                    _logger.LogInformation("Initiating Account Does Not Exist: User Account with {InitatingAccountID} {UtcNow}", transaction.InitatingAccountID, DateTime.UtcNow);
                    return NotFound("Contact Customer Support");
                }

                //TODO : Implement Limit Checks
                initiatingAccount.ServiceAccount.Wallets[0].Balance -= transaction.Amount;
                if (recipientAccount == null)
                {
                    var voucher = new Voucher{ Amount = transaction.Amount, CreatedAt=DateTime.UtcNow, Currency= "KES", ExpiresAt = DateTime.UtcNow.AddDays(14), PayeeMSISDN= transaction.ReceivingMSISDN, Id = Guid.NewGuid().ToString(), State="Initiated", TransactionId = transaction.Id};
                    transaction.Voucher = voucher;
                }
                else
                {
                    recipientAccount.ServiceAccount.Wallets[0].Balance += transaction.Amount;
                    await client.PutAsJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + transaction.ReceivingAccountID), recipientAccount);

                }
                await client.PutAsJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + transaction.InitatingAccountID), initiatingAccount);

                transaction.State = "Pending";
                transaction.CompletedAt = DateTime.UtcNow;
                await _transactionService.UpdateTransaction(transaction.Id, transaction);
                return CreatedAtAction(nameof(Get), new { id = transaction.Id }, transaction);
            }

        }

    }

}