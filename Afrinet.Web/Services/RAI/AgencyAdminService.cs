using RAI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using RAI.API.Models;

namespace RAI.Services;

public class AgencyAdminService
{
    private IMongoCollection<AgencyAdmin> _agencyAdmins;


    public AgencyAdminService(IOptions<RAIDatabaseSettings> rAIDatabaseSettings)
    {

        var mongoClient = new MongoClient(rAIDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(rAIDatabaseSettings.Value.DatabaseName);
        _agencyAdmins = mongoDatabase.GetCollection<AgencyAdmin>("AgencyAdmin");
    }

    public async Task<List<AgencyAdmin>> GetAgencyAdmins() => await _agencyAdmins.Find(_ => true).ToListAsync();
    public async Task<AgencyAdmin> GetAgencyAdmin(string id) => await _agencyAdmins.Find(agencyAdmin => agencyAdmin.Id == id).FirstOrDefaultAsync();
    public async Task CreateAgencyAdmin(AgencyAdmin agencyAdmin) => await _agencyAdmins.InsertOneAsync(agencyAdmin);
    public async Task UpdateAgencyAdmin(string id, AgencyAdmin agencyAdmin) => await _agencyAdmins.ReplaceOneAsync(agencyAdmin => agencyAdmin.Id == id, agencyAdmin);
    public async Task DeleteAgencyAdmin(string id) => await _agencyAdmins.DeleteOneAsync(agencyAdmin => agencyAdmin.Id == id);

}