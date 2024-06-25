using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UPBank.Accounts.Api.Agency;
using UPBank.Accounts.Api.Agency.Abstract;
using UPBank.Accounts.Api.Customer;
using UPBank.Accounts.Api.Customer.Abstract;
using UPBank.Accounts.Data;
using UPBank.Accounts.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UPBankAccountsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UPBankAccountsContext") ?? throw new InvalidOperationException("Connection string 'UPBankAccountsContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSingleton<ICustomerApi, MockCustomerApi>();
builder.Services.AddSingleton<IAgencyApi, MockAgencyApi>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CreditCardService>();
builder.Services.AddScoped<TransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
