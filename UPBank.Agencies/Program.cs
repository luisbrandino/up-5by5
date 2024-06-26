using Microsoft.EntityFrameworkCore;
using UPBank.Agencies.APIs.AddressesAPI;
using UPBank.Agencies.APIs.AddressesAPI.Interface;
using UPBank.Agencies.APIs.EmployeesAPI.Interface;
using UPBank.Agencies.Data;
using UPBank.Agencies.APIs.EmployeesAPI;
using UPBank.Agencies.APIs.AccountsAPI.Interface;
using UPBank.Agencies.APIs.AccountsAPI;
using UPBank.Agencies.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UPBankAgenciesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UPBankAgenciesContext") ?? throw new InvalidOperationException("Connection string 'UPBankAgenciesContext' not found.")));

builder.Services.AddControllers();

builder.Services.AddSingleton<IAddressService, MockAddressService>();
builder.Services.AddSingleton<IEmployeeService, MockEmployeeService>();
builder.Services.AddSingleton<IAccountService, MockAccountService>();

builder.Services.AddScoped<AgencyService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
