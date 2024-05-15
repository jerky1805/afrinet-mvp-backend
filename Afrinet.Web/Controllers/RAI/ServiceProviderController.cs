using Subscribers.API.Models;
using Subscribers.API.Services;
using Microsoft.AspNetCore.Mvc;
using RAI.Models;
using RAI.Services;

namespace RAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ServiceProviderController : ControllerBase
{

    private readonly ServiceProviderService _serviceProviderService;


    private readonly ILogger<ServiceProviderController> _logger;

    public IConfiguration Configuration;


    public ServiceProviderController(
        ServiceProviderService serviceProviderService,
        ILogger<ServiceProviderController> logger,
        IConfiguration configuration
    )
    {
        _serviceProviderService = serviceProviderService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<RAIServiceProvider>> Get() =>
         await _serviceProviderService.GetServiceProviders();

    [HttpGet("{id}")]
    public async Task<ActionResult<RAIServiceProvider>> Get(string id)
    {
        var serviceProvider = await _serviceProviderService.GetServiceProvider(id);

        if (serviceProvider is null)
        {
            _logger.LogInformation("ERROR Finding a Service Provider: {id} from {UtcNow}", id, DateTime.UtcNow);
            return NotFound();
        }
        return serviceProvider;
    }


    [HttpPost]
    public async Task<IActionResult> Post(RAIServiceProvider serviceProvider)
    {
        try
        {

            var s = await _serviceProviderService.GetServiceProvider(serviceProvider.Id);
            if (s is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Service Provider !: {serviceProvider} with {Id} {UtcNow}", serviceProvider, DateTime.UtcNow, serviceProvider.Id);
                return Created("Please Contact Support", serviceProvider);
            }


            serviceProvider.Id = Guid.NewGuid().ToString();
            serviceProvider.CreatedAt = DateTime.Now;

            await _serviceProviderService.CreateSeerviceProvider(serviceProvider);

            return CreatedAtAction(nameof(Get), new { id = serviceProvider.Id }, serviceProvider);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(280, ex, "Error while Adding Service Provider - {serviceProvider}", serviceProvider);
            return Problem("Error while Adding Service Provider");
        }

    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, RAIServiceProvider serviceProvider)
    {

        try
        {
            var s = await _serviceProviderService.GetServiceProvider(id);

            if (s is null)
            {
                _logger.LogInformation("ERROR Finding a Service Provider: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }



            await _serviceProviderService.UpdateServiceProvider(id, serviceProvider);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(281, ex, "Error while Updating Service Provider - {serviceProvider}, {id}", serviceProvider, id);
            return Problem("Error while Updating Service Provider");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var s = await _serviceProviderService.GetServiceProvider(id);

            if (s is null)
            {
                _logger.LogInformation("ERROR Finding a Service Provider: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _serviceProviderService.DeleteServiceProvider(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(283, ex, "Error while Archiving Service Provider -  {id}", id);
            return Problem("Error while Removing Service Provider");
        }

    }

}