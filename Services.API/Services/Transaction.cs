using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.API.Models;
namespace Services.API;

public class TransactionService
{
    private readonly IMongoCollection<Transaction> _transactions;

    public TransactionService(IOptions<TransactionDatabaseSettings> transactionSettings)
    {

        var mongoClient = new MongoClient(transactionSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(transactionSettings.Value.DatabaseName);
        _transactions = mongoDatabase.GetCollection<Transaction>(transactionSettings.Value.TransactionCollectionName); 

    }

    public async Task<List<Transaction>> GetTransactions() => await _transactions.Find(_ => true).ToListAsync();
    public async Task<Transaction> GetTransaction(string id) => await _transactions.Find(Transaction => Transaction.Id == id).FirstOrDefaultAsync(); 
    public async Task CreateTransaction(Transaction transaction) => await _transactions.InsertOneAsync(transaction);
    public async Task UpdateTransaction(string id, Transaction transaction) => await _transactions.ReplaceOneAsync(Transaction => Transaction.Id == id, transaction);
    public async Task DeleteTransaction(string id) => await _transactions.DeleteOneAsync(Transaction => Transaction.Id == id); 

}