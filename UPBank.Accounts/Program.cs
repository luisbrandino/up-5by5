using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UPBank.Accounts.Api;
using UPBank.Accounts.Api.Agency;
using UPBank.Accounts.Api.Agency.Abstract;
using UPBank.Accounts.Api.Customer;
using UPBank.Accounts.Api.Customer.Abstract;
using UPBank.Accounts.Data;
using UPBank.Accounts.Filters;
using UPBank.Accounts.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UPBankAccountsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UPBankAccountsContext") ?? throw new InvalidOperationException("Connection string 'UPBankAccountsContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<AddNumberOriginToTransactionFilter>();

builder.Services.AddSingleton<ICustomerApi, UPBankCostumerApi>();
builder.Services.AddSingleton<IAgencyApi, UPBankAgencyApi>();
builder.Services.AddTransient<IConsumer, HttpClientConsumer>();

builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<CreditCardService>();
builder.Services.AddTransient<TransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
