using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class AgentAccountsService
{
    private readonly IMongoCollection<AgentAccount> _agentAccounts;

    public AgentAccountsService(IOptions<AgentAccountDatabaseSettings> agentAccountSettings)
    {

        var mongoClient = new MongoClient(agentAccountSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(agentAccountSettings.Value.DatabaseName);
        _agentAccounts = mongoDatabase.GetCollection<AgentAccount>(agentAccountSettings.Value.AgentAccountCollectionName);

    }

    public async Task<List<AgentAccount>> GetAgentAccounts() => await _agentAccounts.Find(_ => true).ToListAsync();

    public async Task<List<AgentAccount>> GetAgentAccountsbyBRanchId(string BranchId) => await _agentAccounts.Find(agentAccount => agentAccount.BranchId == BranchId).ToListAsync();

    public async Task<AgentAccount> GetAgentAccount(string id) => await _agentAccounts.Find(agentAccount => agentAccount.Id == id).FirstOrDefaultAsync();
    public async Task<AgentAccount> GetAgentAccountbyMSISDN(string MSISDN) => await _agentAccounts.Find(agentAccount => agentAccount.MSISDN == MSISDN).FirstOrDefaultAsync();

    public async Task CreateAgentAccount(AgentAccount agentAccount) => await _agentAccounts.InsertOneAsync(agentAccount);
    public async Task UpdateAgentAccount(string id, AgentAccount agentAccount) => await _agentAccounts.ReplaceOneAsync(agentAccount => agentAccount.Id == id, agentAccount);
    public async Task DeleteAgentAccount(string id) => await _agentAccounts.DeleteOneAsync(agentAccount => agentAccount.Id == id);

}