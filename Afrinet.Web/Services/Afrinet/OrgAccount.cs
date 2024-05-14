using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class OrgAccountsService
{
    private readonly IMongoCollection<OrgAccount> _orgAccounts;

    public OrgAccountsService(IOptions<OrgAccountDatabaseSettings> orgAccountSettings)
    {

        var mongoClient = new MongoClient(orgAccountSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(orgAccountSettings.Value.DatabaseName);
        _orgAccounts = mongoDatabase.GetCollection<OrgAccount>(orgAccountSettings.Value.OrgAccountCollectionName);

    }

    public async Task<List<OrgAccount>> GetOrgAccounts() => await _orgAccounts.Find(_ => true).ToListAsync();
    public async Task<OrgAccount> GetOrgAccount(string id) => await _orgAccounts.Find(orgAccount => orgAccount.Id == id).FirstOrDefaultAsync();
    public async Task<OrgAccount> GetOrgAccountbyMSISDN(string MSISDN) => await _orgAccounts.Find(orgAccount => orgAccount.MSISDN == MSISDN).FirstOrDefaultAsync();

    public async Task CreateOrgAccount(OrgAccount orgAccount) => await _orgAccounts.InsertOneAsync(orgAccount);
    public async Task UpdateOrgAccount(string id, OrgAccount orgAccount) => await _orgAccounts.ReplaceOneAsync(orgAccount => orgAccount.Id == id, orgAccount);
    public async Task DeleteOrgAccount(string id) => await _orgAccounts.DeleteOneAsync(orgAccount => orgAccount.Id == id);

}