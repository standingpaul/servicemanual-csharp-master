using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using EtteplanMORE.ServiceManual.ApplicationCore.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// MongoDB
builder.Services.Configure<ServiceManualDatabaseSettings>(builder.Configuration.GetSection("ServiceManualDatabase"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IMaintenanceTasksService, MaintenanceTasksService>();

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
