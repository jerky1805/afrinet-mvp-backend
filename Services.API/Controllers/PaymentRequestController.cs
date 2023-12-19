using Services.API.Models;
using Services.API.Services;
using Microsoft.AspNetCore.Mvc;
using Services.API.Service;
using Afrinet.API.Contracts;
using System.Text.Json;

namespace Services.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PaymentRequestController : ControllerBase
{

    private readonly ILogger<PaymentRequestController> _logger;
    private readonly IQueue _queue;


    public PaymentRequestController(
        ILogger<PaymentRequestController> logger,
        IQueue queue 

    )
    {
        _logger = logger;
        _queue = queue;
    }





    [HttpPost]
    public async Task<IActionResult> Post( OrderDetails orderDetails)
    {
        if (orderDetails == null)
            return BadRequest();

       else
        {
            orderDetails.Id = Guid.NewGuid().ToString();
            await _queue.SendMessageAsync(orderDetails);
             return Ok();
        }
       
    }








}