using AutoMapper;
using BLL.Mappers;
using BLL.Services;
using DAL.Data;
using DAL.Interfaces;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string connectionString = 
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped(_ => new MySqlConnection(connectionString));

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<IFlightDestinationRepository, FlightDestinationRepository>();

builder.Services.AddScoped<PassengerService>();
builder.Services.AddScoped<AircraftService>();
builder.Services.AddScoped<AirportService>();
builder.Services.AddScoped<FlightDestinationService>();



var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperProfile());
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.MapControllers();

app.Run();
