using RAI.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using RAI.API.Models;

namespace RAI.Services;

public class StaffService
{
    private IMongoCollection<Staff> _staffs;


    public StaffService(IOptions<RAIDatabaseSettings> rAIDatabaseSettings)
    {

        var mongoClient = new MongoClient(rAIDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(rAIDatabaseSettings.Value.DatabaseName);
        _staffs = mongoDatabase.GetCollection<Staff>("Staff");
    }

    public async Task<List<Staff>> GetStaffs() => await _staffs.Find(_ => true).ToListAsync();
    public async Task<Staff> GetStaff(string id) => await _staffs.Find(staff => staff.Id == id).FirstOrDefaultAsync();
    public async Task CreateStaff(Staff staff) => await _staffs.InsertOneAsync(staff);
    public async Task UpdateStaff(string id, Staff staff) => await _staffs.ReplaceOneAsync(staff => staff.Id == id, staff);
    public async Task DeleteStaff(string id) => await _staffs.DeleteOneAsync(staff => staff.Id == id);

}