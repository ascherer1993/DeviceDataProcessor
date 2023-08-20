using DeviceDataProcessor.DataAccess;
using DeviceDataProcessor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDeviceDataRepository, JsonDeviceDataRepository>();
builder.Services.AddSingleton<IDeviceDataConversionService, DeviceDataConversionService>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();