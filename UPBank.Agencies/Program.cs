using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UPBank.Agencies.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UPBankAgenciesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UPBankAgenciesContext") ?? throw new InvalidOperationException("Connection string 'UPBankAgenciesContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
