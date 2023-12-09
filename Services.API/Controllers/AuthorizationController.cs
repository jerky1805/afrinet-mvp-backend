using Services.API.Models;
using Services.API.Services;
using Microsoft.AspNetCore.Mvc;
using Afrinet.Models;
using Microsoft.Extensions.Configuration;
using Afrinet.Models.Payment;

namespace Services.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthorizationController : ControllerBase
{

    private readonly PaymentService _paymentService;
    private readonly ILogger<AuthorizationController> _logger;
   public IConfiguration Configuration;

    public AuthorizationController(
        PaymentService paymentService,
        ILogger<AuthorizationController> logger,
        IConfiguration configuration
    )
    {
        _paymentService = paymentService; 
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<Payment>> Get() =>
         await _paymentService.GetPayments();

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> Get(string id)
    {
        var payment = await _paymentService.GetPayment(id); 

        if (payment is null)
        {
            _logger.LogInformation("ERROR Finding a Payment: {id} from {Now}", id, DateTime.Now);
            return NotFound();
        } 
        return payment;
    }


    [HttpPost]
    public async Task<IActionResult> Post(Payment payment)
    {
        try
        {   
            
           // await _paymentService.SendMessage(qUrl, payment.ToString());
            payment.Id = Guid.NewGuid().ToString();
            await _paymentService.CreatePayment(payment);
            return CreatedAtAction(nameof(Get), new { id = payment.Id }, payment);
            
        }
        catch (System.Exception ex)
        {
            _logger.LogError(100, ex, "ERROR Creating a Payment: {payment} from {Now}", payment, DateTime.Now);
            return BadRequest(ex.Message);
        }
    }
    
   

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Payment updatedPayment)
    {

        try
        {
            var payment = await _paymentService.GetPayment(id);

            if (payment is null)
            {
                _logger.LogInformation("ERROR Finding Payment : {id} from {Now}", id, DateTime.Now);
                return NotFound();
            }

            updatedPayment.Id = payment.Id;

            await _paymentService.UpdatePayment(id, updatedPayment);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(11, ex, "Error while Updating Payment - {id} - {updatedPayment} - at {Now}", updatedPayment, id, DateTime.Now);
            return Problem("Error while Updating Subscriber");
        }

    }

    

}