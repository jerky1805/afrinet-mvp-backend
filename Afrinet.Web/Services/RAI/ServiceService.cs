using RAI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using RAI.API.Models;

namespace RAI.Services;

public class ServiceService
{
    private IMongoCollection<Service> _services;


    public ServiceService(IOptions<RAIDatabaseSettings> rAIDatabaseSettings)
    {

        var mongoClient = new MongoClient(rAIDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(rAIDatabaseSettings.Value.DatabaseName);
        _services = mongoDatabase.GetCollection<Service>("Services");
    }

    public async Task<List<Service>> GetServices() => await _services.Find(_ => true).ToListAsync();
    public async Task<Service> GetService(string id) => await _services.Find(service => service.Id == id).FirstOrDefaultAsync();
    public async Task CreateService(Service service) => await _services.InsertOneAsync(service);
    public async Task UpdateService(string id, Service service) => await _services.ReplaceOneAsync(service => service.Id == id, service);
    public async Task DeleteService(string id) => await _services.DeleteOneAsync(service => service.Id == id);

}