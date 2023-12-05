using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class WalletsService
{
    private readonly IMongoCollection<Wallet> _wallets;

    public WalletsService(IOptions<WalletDatabaseSettings> walletSettings)
    {

        var mongoClient = new MongoClient(walletSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(walletSettings.Value.DatabaseName);
        _wallets = mongoDatabase.GetCollection<Wallet>(walletSettings.Value.WalletCollectionName); 

    }

    public async Task<List<Wallet>> GetWallets() => await _wallets.Find(_ => true).ToListAsync();
    public async Task<Wallet> GetWallet(string id) => await _wallets.Find(Wallet => Wallet.Id == id).FirstOrDefaultAsync(); 
    public async Task CreateWallet(Wallet wallet) => await _wallets.InsertOneAsync(wallet);
    public async Task UpdateWallet(string id, Wallet wallet) => await _wallets.ReplaceOneAsync(Wallet => Wallet.Id == id, wallet);
    public async Task DeleteWallet(string id) => await _wallets.DeleteOneAsync(Wallet => Wallet.Id == id); 


}