using Agents.DotnetAgents;
using Agents.Interfaces;
using GameRunner;
using GameServer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:
    "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Game} {Faction} {ScriptId} - {Message}{NewLine}{Exception}")
    .WriteTo.Trace(outputTemplate:
    "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Game} {Faction} {ScriptId} - {Message}{NewLine}{Exception}")
    // Add more Serilog configuration here if needed
    .Enrich.FromLogContext()
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAgentFactory, AgentFactory>();
builder.Services.AddScoped<IRunner, Runner>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Host.UseSerilog();

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

app.UseSerilogRequestLogging();

app.Run();
Log.CloseAndFlush();
