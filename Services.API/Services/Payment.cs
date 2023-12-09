using Afrinet.Models;
using Afrinet.Models.Payment;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.API.Models;

namespace Services.API.Services;

public class PaymentService
{

    private readonly IMongoCollection<Payment> _payments;


    public PaymentService(IOptions<PaymentDatabaseSettings> paymentSettings)
    {

        var mongoClient = new MongoClient(paymentSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(paymentSettings.Value.DatabaseName);
        _payments = mongoDatabase.GetCollection<Payment>(paymentSettings.Value.PaymentsCollectionName);
    }

    public async Task<List<Payment>> GetPayments() => await _payments.Find(_ => true).ToListAsync();
    public async Task<Payment> GetPayment(string id) => await _payments.Find(Payment => Payment.Id == id).FirstOrDefaultAsync();
    public async Task CreatePayment(Payment payment) => await _payments.InsertOneAsync(payment);
    public async Task UpdatePayment(string id, Payment payment) => await _payments.ReplaceOneAsync(Payment => Payment.Id == id, payment);
    public async Task DeletePayment(string id) => await _payments.DeleteOneAsync(Payment => Payment.Id == id);


}