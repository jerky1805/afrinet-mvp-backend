using Services.API.Models;
using Services.API.Services;
using Microsoft.AspNetCore.Mvc;
using Services.API.Service;
using Afrinet.API.Contracts;
using System.Text.Json;
using Afrinet.Models;
using System.Runtime.Intrinsics.Arm;

namespace Services.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ActivationController : ControllerBase
{

    private readonly ILogger<ActivationController> _logger;
    private readonly IQueue _queue;

    private readonly ActivationService _activationService;



    public ActivationController(
        ILogger<ActivationController> logger,
        IQueue queue,
        ActivationService activationService

    )
    {
        _logger = logger;
        _queue = queue;
        _activationService = activationService;
    }





    [HttpGet("{id}")]
    public async Task<ActionResult<ActivationRequest>> Get(string id)
    {
        var activationRequest = await _activationService.GetActivation(id);

        if (activationRequest is null)
        {
            _logger.LogInformation("ERROR Finding an Activation Request: {id} from {Now}", id, DateTime.UtcNow);
            return NotFound();
        }
        return activationRequest;
    }


    [HttpPost]
    public async Task<IActionResult> Post(ActivationRequest activationRequest)
    {
        if (activationRequest == null)
            return BadRequest();

        else
        {
            activationRequest.Id = Guid.NewGuid().ToString();
            await _queue.SendMessageAsync(activationRequest);
            return Ok("Activation Request Sent");
        }

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, ActivationRequest activationRequest)
    {
        if (activationRequest == null)
            return BadRequest();

        else
        {

            using (var client = new HttpClient())
            {
                //TODO : Make URL a configuration
                //client.BaseAddress = new Uri("http://localhost:5122/api/Activation");
                var ar = await client.GetFromJsonAsync<ActivationRequest>(new Uri("http://localhost:5122/api/Activation/" + id));

                if (ar == null)
                {
                    return NotFound("Activation Request Not found Or Expired");
                }
                else
                {
                    if (activationRequest.TOTP == ar.TOTP && activationRequest.SubscriberId == ar.SubscriberId && ar.Status == "Initiated")
                    {
                        // client.BaseAddress = ;
                        var userAccount = await client.GetFromJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + ar.SubscriberId));
                        if (userAccount == null)
                        {
                            return BadRequest("Subscriber Not Found");
                        }
                        userAccount.Status = "Active";
                        userAccount.PIN = activationRequest.PIN;
                        userAccount.PINChangedAt = DateTime.UtcNow;
                        userAccount.UpdatedAt = DateTime.UtcNow;
                        var putTask = client.PutAsJsonAsync<UserAccount>(new Uri("http://localhost:5032/api/Subscribers/" + ar.SubscriberId), userAccount);
                        putTask.Wait();
                        var result = putTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            ar.Status = "Completed";
                            ar.UpdatedAt = DateTime.UtcNow;
                           await  _activationService.UpdateActivation(ar.Id, ar);
                            return Ok("Account Activated Successfully");
                        }
                        else
                        {
                            return BadRequest("Account Activation Failed");

                        }
                    }
                    else
                    {
                        return BadRequest("Invalid Activation Request");

                    }
                }


            }
        }

    }








}