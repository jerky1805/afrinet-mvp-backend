using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class PersonalDetailsService
{
    private readonly IMongoCollection<PersonalDetails> _personalDetails;

    public PersonalDetailsService(IOptions<PersonalDetailsDatabaseSettings> personalDetailsSettings)
    {

        var mongoClient = new MongoClient(personalDetailsSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(personalDetailsSettings.Value.DatabaseName);
        _personalDetails = mongoDatabase.GetCollection<PersonalDetails>(personalDetailsSettings.Value.PersonalDetailsCollectionName);

    }

    public async Task<List<PersonalDetails>> GetPersonalDetails() => await _personalDetails.Find(_ => true).ToListAsync();
    public async Task<PersonalDetails> GetPersonalDetail(string id) => await _personalDetails.Find(personalDetails => personalDetails.Id == id).FirstOrDefaultAsync();
    public async Task CreatePersonalDetail(PersonalDetails personalDetails) => await _personalDetails.InsertOneAsync(personalDetails);
    public async Task UpdatePersonalDetail(string id, PersonalDetails personalDetails) => await _personalDetails.ReplaceOneAsync(personalDetails => personalDetails.Id == id, personalDetails);
    public async Task DeletePersonalDetail(string id) => await _personalDetails.DeleteOneAsync(personalDetails => personalDetails.Id == id);

}