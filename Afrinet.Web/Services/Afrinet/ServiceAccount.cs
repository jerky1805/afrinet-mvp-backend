using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class ServiceAccountsService 
{

    private readonly IMongoCollection<ServiceAccount> _serviceAccount;

    public ServiceAccountsService(IOptions<ServiceAccountDatabaseSettings> serviceAccountSettings)
    {
        var mongoClient = new MongoClient(serviceAccountSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(serviceAccountSettings.Value.DatabaseName);
        _serviceAccount = mongoDatabase.GetCollection<ServiceAccount>(serviceAccountSettings.Value.ServiceAccountCollectionName);
    
    }

    public async Task<List<ServiceAccount>> GetServiceAccounts() => await _serviceAccount.Find(_ => true).ToListAsync();
    public async Task<ServiceAccount> GetServiceAccount(string id) => await _serviceAccount.Find(serviceAccount => serviceAccount.Id == id).FirstOrDefaultAsync();

    public async Task CreateServiceAccount(ServiceAccount serviceAccount) => await _serviceAccount.InsertOneAsync(serviceAccount);

    public async Task UpdateServiceAccount(string id, ServiceAccount serviceAccount) => await _serviceAccount.ReplaceOneAsync(serviceAccount => serviceAccount.Id == id, serviceAccount);

    public async Task DeleteServiceAccount(string id) => await _serviceAccount.DeleteOneAsync(serviceAccount => serviceAccount.Id == id);

}