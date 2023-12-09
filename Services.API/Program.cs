using Afrinet.Models;
using Services.API.Models;
using Services.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<PaymentDatabaseSettings> (
    builder.Configuration.GetSection("PaymentDatabase"));

builder.Services.AddSingleton<PaymentService>();

builder.Services.AddScoped<IServiceBus, ServiceBus>();

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
