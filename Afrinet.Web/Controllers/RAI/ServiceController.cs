
using Microsoft.AspNetCore.Mvc;
using RAI.Models;
using RAI.Services;

namespace RAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ServiceController : ControllerBase
{

    private readonly ServiceService _serviceService;


    private readonly ILogger<ServiceController> _logger;

    public IConfiguration Configuration;


    public ServiceController(
        ServiceService serviceService,
        ILogger<ServiceController> logger,
        IConfiguration configuration
    )
    {
        _serviceService = serviceService;
        _logger = logger;
        Configuration = configuration;
    }


    [HttpGet]
    public async Task<List<Service>> Get() =>
         await _serviceService.GetServices();

    [HttpGet("{id}")]
    public async Task<ActionResult<Service>> Get(string id)
    {
        var service = await _serviceService.GetService(id);

        if (service is null)
        {
            _logger.LogInformation("ERROR Finding a Service: {id} from {Now}", id, DateTime.UtcNow);
            return NotFound();
        }
        return service;
    }


    [HttpPost]
    public async Task<IActionResult> Post(Service service)
    {
        try
        {

            var s = await _serviceService.GetService(service.Id);
            if (s is not null)
            {
                _logger.LogInformation("Attempt to Create a Duplicate Service !: {service} with {Id} {UtcNow}", service, DateTime.UtcNow, service.Id);
                return Created("Please Contact Support", service);
            }


            service.Id = Guid.NewGuid().ToString();
            service.CreatedAt = DateTime.Now;

            await _serviceService.CreateService(service);

            return CreatedAtAction(nameof(Get), new { id = service.Id }, service);
        }
        catch (System.Exception ex)
        {

            _logger.LogError(250, ex, "Error while Adding Service - {service}", service);
            return Problem("Error while Adding Service");
        }

    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Service service)
    {

        try
        {
            var s = await _serviceService.GetService(id);

            if (s is null)
            {
                _logger.LogInformation("ERROR Finding a Service: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }


            service.UpdatedAt = DateTime.UtcNow;
            await _serviceService.UpdateService(id, service);

            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(251, ex, "Error while Updating service - {service}, {id}", service, id);
            return Problem("Error while Updating Service");
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var s = await _serviceService.GetService(id);

            if (s is null)
            {
                _logger.LogInformation("ERROR Finding a Service: {id} from {UtcNow}", id, DateTime.UtcNow);
                return NotFound();
            }
            //TODO - Check if there are any active Branches. 
            await _serviceService.DeleteService(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {

            _logger.LogError(253, ex, "Error while Archiving Service -  {id}", id);
            return Problem("Error while Removing Service");
        }

    }

}