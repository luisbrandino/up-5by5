using Microsoft.AspNetCore.Connections;
using UPBank.Customers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UPBank.Customers.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UPBankCustomersContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UPBankCustomersContext") ?? throw new InvalidOperationException("Connection string 'UPBankCustomersContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<CustomerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
