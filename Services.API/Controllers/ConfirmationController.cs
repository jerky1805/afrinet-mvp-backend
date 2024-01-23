using Services.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;

namespace Services.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ConfirmationController : ControllerBase
{

    private readonly ILogger<ConfirmationController> _logger;
    private readonly IQueue _queue;

    private readonly ConfirmationService _confirmationService;



    public ConfirmationController(
        ILogger<ConfirmationController> logger,
        IQueue queue,
        ConfirmationService confirmationService

    )
    {
        _logger = logger;
        _queue = queue;
        _confirmationService = confirmationService;
    }





    [HttpGet("{id}")]
    public async Task<ActionResult<ConfirmationRequest>> Get(string id)
    {
        var confirmationRequest = await _confirmationService.GetConfirmation(id);

        if (confirmationRequest is null)
        {
            _logger.LogInformation("ERROR Finding an Confirmation Request: {id} from {Now}", id, DateTime.UtcNow);
            return NotFound();
        }
        return confirmationRequest;
    }


    [HttpPost]
    public async Task<IActionResult> Post(ConfirmationRequest confirmationRequest)
    {
        if (confirmationRequest == null)
            return BadRequest();

        else
        {
            confirmationRequest.Id = Guid.NewGuid().ToString();
            await _queue.SendMessageAsync(confirmationRequest);
            return Ok("Confirmation Request Sent");
        }

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, ConfirmationRequest confirmationRequest)
    {
        if (confirmationRequest == null)
            return BadRequest();

        else
        {

            using (var client = new HttpClient())
            {
                return Ok("Confirmation Request Sent");
                // //TODO : Make URL a configuration
                // //client.BaseAddress = new Uri("http://localhost:5122/api/Activation");
                // var ar = await client.GetFromJsonAsync<ActivationRequest>(new Uri("http://localhost:5122/api/Activation/" + id));

                // if (ar == null)
                // {
                //     return NotFound("Activation Request Not found Or Expired");
                // }
                // else
                // {
                //     if (activationRequest.TOTP == ar.TOTP && activationRequest.SubscriberId == ar.SubscriberId && ar.Status == "Initiated")
                //     {
                //         // client.BaseAddress = ;
                //         var userAccount = await client.GetFromJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + ar.SubscriberId));
                //         if (userAccount == null)
                //         {
                //             return BadRequest("Subscriber Not Found");
                //         }
                //         userAccount.Status = "Active";
                //         userAccount.PIN = activationRequest.PIN;
                //         userAccount.PINChangedAt = DateTime.UtcNow;
                //         userAccount.UpdatedAt = DateTime.UtcNow;
                //         var putTask = client.PutAsJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + ar.SubscriberId), userAccount);
                //         putTask.Wait();
                //         var result = putTask.Result;
                //         if (result.IsSuccessStatusCode)
                //         {
                //             ar.Status = "Completed";
                //             ar.UpdatedAt = DateTime.UtcNow;
                //            await  _activationService.UpdateActivation(ar.Id, ar);
                //             return Ok("Account Activated Successfully");
                //         }
                //         else
                //         {
                //             return BadRequest("Account Activation Failed");

                //         }
                //     }
                //     else
                //     {
                //         return BadRequest("Invalid Activation Request");

                //     }
                // }


            }
        }

    }








}