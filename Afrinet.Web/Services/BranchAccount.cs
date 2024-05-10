using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class BranchAccountsService
{
    private readonly IMongoCollection<Branch> _branchAccounts;

    public BranchAccountsService(IOptions<BranchAccountDatabaseSettings> branchAccountSettings)
    {

        var mongoClient = new MongoClient(branchAccountSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(branchAccountSettings.Value.DatabaseName);
        _branchAccounts = mongoDatabase.GetCollection<Branch>(branchAccountSettings.Value.BranchAccountCollectionName);

    }

    public async Task<List<Branch>> GetBranchAccounts() => await _branchAccounts.Find(_ => true).ToListAsync();

    public async Task<List<Branch>> GetBranchAccountsbyHeadOfficeId(string HeadOfficeId) => await _branchAccounts.Find(branchAccount => branchAccount.HeadOfficeId == HeadOfficeId).ToListAsync();
    public async Task<Branch> GetBranchAccount(string id) => await _branchAccounts.Find(branchAccount => branchAccount.Id == id).FirstOrDefaultAsync();
    public async Task<Branch> GetBranchAccountbyMSISDN(string MSISDN) => await _branchAccounts.Find(branchAccount => branchAccount.MSISDN == MSISDN).FirstOrDefaultAsync();
    public async Task CreateBranchAccount(Branch branchAccount) => await _branchAccounts.InsertOneAsync(branchAccount);
    public async Task UpdateBranchAccount(string id, Branch branchAccount) => await _branchAccounts.ReplaceOneAsync(branchAccount => branchAccount.Id == id, branchAccount);
    public async Task DeleteBranchAccount(string id) => await _branchAccounts.DeleteOneAsync(branchAccount => branchAccount.Id == id);

}