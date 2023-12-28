using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Activation.Processor.Models;

namespace Activation.Processor;

public class ActivationService
{
    private readonly IMongoCollection<ActivationRequest> _activations;

    public ActivationService(IOptions<ActivationDatabaseSettings> activationSettings)
    {

        var mongoClient = new MongoClient(activationSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(activationSettings.Value.DatabaseName);
        _activations = mongoDatabase.GetCollection<ActivationRequest>(activationSettings.Value.ActivationCollectionName); 

    }

    public async Task<List<ActivationRequest>> GetActivations() => await _activations.Find(_ => true).ToListAsync();
    public async Task<ActivationRequest> GetActivation(string id) => await _activations.Find(Activation => Activation.Id == id).FirstOrDefaultAsync(); 
    public async Task CreateActivation(ActivationRequest activation) => await _activations.InsertOneAsync(activation);
    public async Task UpdateActivation(string id, ActivationRequest activation) => await _activations.ReplaceOneAsync(Activation => Activation.Id == id, activation);
    public async Task DeleteActivation(string id) => await _activations.DeleteOneAsync(Activation => Activation.Id == id); 

}