using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.API.Models;
namespace Services.API;

public class ConfirmationService
{
    private readonly IMongoCollection<ConfirmationRequest> _confirmations;

    public ConfirmationService(IOptions<ConfirmationDatabaseSettings> confirmationSettings)
    {

        var mongoClient = new MongoClient(confirmationSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(confirmationSettings.Value.DatabaseName);
        _confirmations = mongoDatabase.GetCollection<ConfirmationRequest>(confirmationSettings.Value.ConfirmationCollectionName); 

    }

    public async Task<List<ConfirmationRequest>> GetConfirmations() => await _confirmations.Find(_ => true).ToListAsync();
    public async Task<ConfirmationRequest> GetConfirmation(string id) => await _confirmations.Find(Confirmation => Confirmation.Id == id).FirstOrDefaultAsync(); 
    public async Task CreateConfirmation(ConfirmationRequest confirmation) => await _confirmations.InsertOneAsync(confirmation);
    public async Task UpdateConfirmation(string id, ConfirmationRequest confirmation) => await _confirmations.ReplaceOneAsync(Confirmation => Confirmation.Id == id, confirmation);
    public async Task DeleteConfirmation(string id) => await _confirmations.DeleteOneAsync(Confirmation => Confirmation.Id == id); 

}