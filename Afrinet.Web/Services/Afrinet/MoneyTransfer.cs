using Afrinet.Models;
using IMT.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class MoneyTransferService
{
    private readonly IMongoCollection<MoneyTransferViewModel> _moneyTransferViewModel;

    public MoneyTransferService(IOptions<MoneyTransferDatabaseSettings> moneyTransferSettings)
    {

        var mongoClient = new MongoClient(moneyTransferSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(moneyTransferSettings.Value.DatabaseName);
        _moneyTransferViewModel = mongoDatabase.GetCollection<MoneyTransferViewModel>(moneyTransferSettings.Value.MoneyTransferCollectionName);

    }

    public async Task<List<MoneyTransferViewModel>> GetMoneyTransfers() => await _moneyTransferViewModel.Find(_ => true).ToListAsync();
    public async Task<MoneyTransferViewModel> GetMoneyTransfer(string id) => await _moneyTransferViewModel.Find(moneyTransfer => moneyTransfer.Id == id).FirstOrDefaultAsync();
    public async Task CreateMoneyTransfer(MoneyTransferViewModel newMoneyTransfer) => await _moneyTransferViewModel.InsertOneAsync(newMoneyTransfer);
    public async Task UpdateMoneyTransfer(string id, MoneyTransferViewModel updatedMoneyTransfer) => await _moneyTransferViewModel.ReplaceOneAsync(moneyTransfer => moneyTransfer.Id == id, updatedMoneyTransfer);
    public async Task DeleteMoneyTransfer(string id) => await _moneyTransferViewModel.DeleteOneAsync(moneyTransfer => moneyTransfer.Id == id);

}