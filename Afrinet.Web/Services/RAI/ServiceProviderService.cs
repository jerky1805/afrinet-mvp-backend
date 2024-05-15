using RAI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using RAI.API.Models;

namespace RAI.Services;

public class ServiceProviderService
{
    private IMongoCollection<RAIServiceProvider> _serviceProviders;


    public ServiceProviderService(IOptions<RAIDatabaseSettings> rAIDatabaseSettings)
    {

        var mongoClient = new MongoClient(rAIDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(rAIDatabaseSettings.Value.DatabaseName);
        _serviceProviders = mongoDatabase.GetCollection<RAIServiceProvider>("ServiceProvider");
    }

    public async Task<List<RAIServiceProvider>> GetServiceProviders() => await _serviceProviders.Find(_ => true).ToListAsync();
    public async Task<RAIServiceProvider> GetServiceProvider(string id) => await _serviceProviders.Find(serviceProvider => serviceProvider.Id == id).FirstOrDefaultAsync();
    public async Task CreateSeerviceProvider(RAIServiceProvider serviceProvider) => await _serviceProviders.InsertOneAsync(serviceProvider);
    public async Task UpdateServiceProvider(string id, RAIServiceProvider serviceProvider) => await _serviceProviders.ReplaceOneAsync(serviceProvider => serviceProvider.Id == id, serviceProvider);
    public async Task DeleteServiceProvider(string id) => await _serviceProviders.DeleteOneAsync(serviceProvider => serviceProvider.Id == id);

}