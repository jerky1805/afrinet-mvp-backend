using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class UserAccountsService
{
    private readonly IMongoCollection<UserAccount> _userAccounts;

    public UserAccountsService(IOptions<UserAccountDatabaseSettings> userAccountSettings)
    {

        var mongoClient = new MongoClient(userAccountSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(userAccountSettings.Value.DatabaseName);
        _userAccounts = mongoDatabase.GetCollection<UserAccount>(userAccountSettings.Value.UserAccountCollectionName);

    }

    public async Task<List<UserAccount>> GetUserAccounts() => await _userAccounts.Find(_ => true).ToListAsync();
    public async Task<UserAccount> GetUserAccount(string id) => await _userAccounts.Find(userAccount => userAccount.Id == id).FirstOrDefaultAsync();
    public async Task<UserAccount> GetUserAccountbyMSISDN(string MSISDN) => await _userAccounts.Find(userAccount => userAccount.MSISDN == MSISDN).FirstOrDefaultAsync();

    public async Task CreateUserAccount(UserAccount userAccount) => await _userAccounts.InsertOneAsync(userAccount);
    public async Task UpdateUserAccount(string id, UserAccount userAccount) => await _userAccounts.ReplaceOneAsync(userAccount => userAccount.Id == id, userAccount);
    public async Task DeleteUserAccount(string id) => await _userAccounts.DeleteOneAsync(userAccount => userAccount.Id == id);

}