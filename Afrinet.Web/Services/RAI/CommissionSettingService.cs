using RAI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using RAI.API.Models;
using Afrinet.Models;

namespace RAI.Services;

public class CommissionSettingService
{
    private IMongoCollection<CommissionSetting> _commissionSettings;


    public CommissionSettingService(IOptions<RAIDatabaseSettings> rAIDatabaseSettings)
    {

        var mongoClient = new MongoClient(rAIDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(rAIDatabaseSettings.Value.DatabaseName);
        _commissionSettings = mongoDatabase.GetCollection<CommissionSetting>("CommissionSetting");
    }

    public async Task<List<CommissionSetting>> GetCommissionSettings() => await _commissionSettings.Find(_ => true).ToListAsync();
    public async Task<CommissionSetting> GetCommissionSetting(string id) => await _commissionSettings.Find(commissionSetting => commissionSetting.Id == id).FirstOrDefaultAsync();
    public async Task CreateCommissionSetting(CommissionSetting commissionSetting) => await _commissionSettings.InsertOneAsync(commissionSetting);
    public async Task UpdateCommissionSetting(string id, CommissionSetting commissionSetting) => await _commissionSettings.ReplaceOneAsync(commissionSetting => commissionSetting.Id == id, commissionSetting);
    public async Task DeleteCommissionSetting(string id) => await _commissionSettings.DeleteOneAsync(commissionSetting => commissionSetting.Id == id);

}