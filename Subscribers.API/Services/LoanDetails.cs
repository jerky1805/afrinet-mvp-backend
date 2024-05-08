using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class LoanDetailsService
{
    private readonly IMongoCollection<LoanDetails> _loanDetails;

    public LoanDetailsService(IOptions<LoanDetailsDatabaseSettings> loanDetailsSettings)
    {

        var mongoClient = new MongoClient(loanDetailsSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(loanDetailsSettings.Value.DatabaseName);
        _loanDetails = mongoDatabase.GetCollection<LoanDetails>(loanDetailsSettings.Value.LoanDetailsCollectionName);

    }

    public async Task<List<LoanDetails>> GetLoanDetails() => await _loanDetails.Find(_ => true).ToListAsync();
    public async Task<LoanDetails> GetLoanDetail(string id) => await _loanDetails.Find(loanDetails => loanDetails.Id == id).FirstOrDefaultAsync();
    public async Task CreateLoanDetail(LoanDetails loanDetails) => await _loanDetails.InsertOneAsync(loanDetails);
    public async Task UpdateLoanDetail(string id, LoanDetails loanDetails) => await _loanDetails.ReplaceOneAsync(loanDetails => loanDetails.Id == id, loanDetails);
    public async Task DeleteLoanDetail(string id) => await _loanDetails.DeleteOneAsync(loanDetails => loanDetails.Id == id);

}