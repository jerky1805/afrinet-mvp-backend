using RAI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using RAI.API.Models;

namespace RAI.Services;

public class AgencyService
{
    private IMongoCollection<Agency> _agencies;


    public AgencyService(IOptions<RAIDatabaseSettings> rAIDatabaseSettings)
    {

        var mongoClient = new MongoClient(rAIDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(rAIDatabaseSettings.Value.DatabaseName);
        _agencies = mongoDatabase.GetCollection<Agency>("Agency");
    }

    public async Task<List<Agency>> GetAgencies() => await _agencies.Find(_ => true).ToListAsync();
    public async Task<Agency> GetAgency(string id) => await _agencies.Find(agency => agency.Id == id).FirstOrDefaultAsync();
    public async Task CreateAgency(Agency agency) => await _agencies.InsertOneAsync(agency);
    public async Task UpdateAgency(string id, Agency agency) => await _agencies.ReplaceOneAsync(agency => agency.Id == id, agency);
    public async Task DeleteAgency(string id) => await _agencies.DeleteOneAsync(agency => agency.Id == id);

}