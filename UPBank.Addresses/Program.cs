using UPBank.Addresses.Mongo.Repositories;
using UPBank.Addresses.Mongo.Settings;
using UPBank.Addresses.PostalServices;
using UPBank.Addresses.PostalServices.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSingleton<IPostalAddressService, ViaCepService>();
builder.Services.AddSingleton<IMongoDatabaseSettings, MongoDatabaseSettings>();
builder.Services.AddSingleton<AddressRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
