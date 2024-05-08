using Afrinet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Subscribers.API.Models;

namespace Subscribers.API.Services;

public class CompanyDetailsService
{
    private readonly IMongoCollection<CompanyDetails> _companyDetails;

    public CompanyDetailsService(IOptions<CompanyDetailsDatabaseSettings> companyDetailsSettings)
    {

        var mongoClient = new MongoClient(companyDetailsSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(companyDetailsSettings.Value.DatabaseName);
        _companyDetails = mongoDatabase.GetCollection<CompanyDetails>(companyDetailsSettings.Value.CompanyDetailsCollectionName);

    }

    public async Task<List<CompanyDetails>> GetCompanyDetails() => await _companyDetails.Find(_ => true).ToListAsync();
    public async Task<CompanyDetails> GetCompanyDetail(string id) => await _companyDetails.Find(companyDetails => companyDetails.Id == id).FirstOrDefaultAsync();
    public async Task CreateCompanyDetail(CompanyDetails companyDetails) => await _companyDetails.InsertOneAsync(companyDetails);
    public async Task UpdateCompanyDetail(string id, CompanyDetails companyDetails) => await _companyDetails.ReplaceOneAsync(companyDetails => companyDetails.Id == id, companyDetails);
    public async Task DeleteCompanyDetail(string id) => await _companyDetails.DeleteOneAsync(companyDetails => companyDetails.Id == id);

}