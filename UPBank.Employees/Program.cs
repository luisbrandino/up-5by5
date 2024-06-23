using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UPBank.Employees.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UPBankEmployeesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UPBankEmployeesContext") ?? throw new InvalidOperationException("Connection string 'UPBankEmployeesContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
