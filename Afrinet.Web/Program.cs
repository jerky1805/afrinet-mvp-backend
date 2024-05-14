
// using Afrinet.Web.Data;
using Auth0.AspNetCore.Authentication;
using MudBlazor.Services;
using Subscribers.API.Models;
using Subscribers.API.Services;
using Afrinet.Web;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
});


// Add services to the container.
builder.Services.AddMudServices();
builder.Services
    .AddAuth0WebAppAuthentication(options =>
    {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
    });
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();


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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger(option =>
    // {
    //     option.RouteTemplate = "codeMaze/{documentName}/swagger.json";
    // });

    // app.UseSwaggerUI(option =>
    //     {
    //         option.SwaggerEndpoint("/codeMaze/v1/swagger.json", "CodeMaze API");
    //         option.RoutePrefix = "codeMaze";
    //     });
    // app.UseExceptionHandler("/Error");
    // // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();
app.MapRazorPages();


app.Run();
