using Afrinet.Models;
using Services.API;
using Services.API.Models;
using Services.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<PaymentDatabaseSettings>(
    builder.Configuration.GetSection("PaymentDatabase"));

builder.Services.Configure<ActivationDatabaseSettings>(
    builder.Configuration.GetSection("ActivationDatabase"));

builder.Services.Configure<TransactionDatabaseSettings>(
    builder.Configuration.GetSection("TransactionDatabase"));

builder.Services.AddSingleton<PaymentService>();
builder.Services.AddSingleton<ActivationService>();
builder.Services.AddSingleton<TransactionService>();


builder.Services.AddScoped<IQueue, Queue>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
