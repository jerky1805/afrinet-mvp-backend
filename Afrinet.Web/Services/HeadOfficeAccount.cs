using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class HeadOfficeAccountsService
{
    private readonly IMongoCollection<HeadOffice> _headOfficeAccounts;

    public HeadOfficeAccountsService(IOptions<HeadOfficeAccountDatabaseSettings> headOfficeAccountSettings)
    {

        var mongoClient = new MongoClient(headOfficeAccountSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(headOfficeAccountSettings.Value.DatabaseName);
        _headOfficeAccounts = mongoDatabase.GetCollection<HeadOffice>(headOfficeAccountSettings.Value.HeadOfficeAccountCollectionName);

    }

    public async Task<List<HeadOffice>> GetHeadOfficeAccounts() => await _headOfficeAccounts.Find(_ => true).ToListAsync();
    public async Task<HeadOffice> GetHeadOfficeAccount(string id) => await _headOfficeAccounts.Find(headOfficeAccount => headOfficeAccount.Id == id).FirstOrDefaultAsync();
    public async Task<HeadOffice> GetHeadOfficeAccountbyMSISDN(string MSISDN) => await _headOfficeAccounts.Find(headOfficeAccount => headOfficeAccount.MSISDN == MSISDN).FirstOrDefaultAsync();

    public async Task CreateHeadOfficeAccount(HeadOffice headOfficeAccount) => await _headOfficeAccounts.InsertOneAsync(headOfficeAccount);
    public async Task UpdateHeadOfficeAccount(string id, HeadOffice headOfficeAccount) => await _headOfficeAccounts.ReplaceOneAsync(headOfficeAccount => headOfficeAccount.Id == id, headOfficeAccount);
    public async Task DeleteHeadOfficeAccount(string id) => await _headOfficeAccounts.DeleteOneAsync(headOfficeAccount => headOfficeAccount.Id == id);

}