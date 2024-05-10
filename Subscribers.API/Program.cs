using Subscribers.API.Models;
using Subscribers.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceAccountDatabaseSettings>(
    builder.Configuration.GetSection("ServiceAccountDatabase"));

builder.Services.Configure<UserAccountDatabaseSettings>(
    builder.Configuration.GetSection("UserAccountDatabase"));

builder.Services.Configure<WalletDatabaseSettings>(
    builder.Configuration.GetSection("WalletDatabase"));

builder.Services.Configure<OrgAccountDatabaseSettings>(
    builder.Configuration.GetSection("OrgAccountDatabase"));

builder.Services.Configure<HeadOfficeAccountDatabaseSettings>(
    builder.Configuration.GetSection("HeadOfficeAccountDatabase"));

builder.Services.Configure<BranchAccountDatabaseSettings>(
    builder.Configuration.GetSection("BranchAccountDatabase"));

builder.Services.Configure<AgentAccountDatabaseSettings>(
    builder.Configuration.GetSection("AgentAccountDatabase"));


builder.Services.Configure<LoanDetailsDatabaseSettings>(
    builder.Configuration.GetSection("LoanDetailsDatabase"));

builder.Services.AddSingleton<ServiceAccountsService>();
builder.Services.AddSingleton<UserAccountsService>();
builder.Services.AddSingleton<WalletsService>();
builder.Services.AddSingleton<OrgAccountsService>();
builder.Services.AddSingleton<HeadOfficeAccountsService>();
builder.Services.AddSingleton<BranchAccountsService>();
builder.Services.AddSingleton<AgentAccountsService>();
builder.Services.AddSingleton<LoanDetailsService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
