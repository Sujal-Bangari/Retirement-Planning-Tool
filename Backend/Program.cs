using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using retplann.Data;
using retplann.Services;
using retplann.Services.Interfaces;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(loggingBuilder=>
{
    loggingBuilder.AddConsole();
});
//builder.Logging.CleanProviders();
//builder.Logging.AddFile("logs/log.txt");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console()
//     .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
//     .CreateLogger();

// builder.Host.UseSerilog();
// builder.Logging.ClearProviders();
// builder.Logging.AddSerilog();
builder.Services.AddSingleton<Repository>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyConnection");
    var logger = sp.GetRequiredService<ILogger<Repository>>();
    return new Repository(connectionString, logger);
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
//builder.Services.AddSingleton(new Repository(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddSingleton<Repository>();
builder.Services.AddScoped<IValidateService, ValidateService>();
builder.Services.AddScoped<IGoal_planService, Goal_planService>();
builder.Services.AddScoped<IFinancialYearService, FinancialYearService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseSerilogRequestLogging();
app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.MapControllers();
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
// });
app.Run();

