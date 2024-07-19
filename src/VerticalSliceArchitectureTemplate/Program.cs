using System.Reflection;
using Carter;
using FluentValidation;
using VerticalSliceArchitectureTemplate;
using VerticalSliceArchitectureTemplate.Features.Tarefas.Domain;
using VerticalSliceArchitectureTemplate.Host;

var appAssembly = Assembly.GetExecutingAssembly();
var builder = WebApplication.CreateBuilder(args);

// Common
builder.Services.AddEfCore();

// Host
builder.Services.AddHandlers();
builder.Services.AddBehaviors();
builder.Services.AddSwaggerGen( options =>
{
    options.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.Ordinal));
});

builder.Services.AddValidatorsFromAssemblyContaining<TarefaValidator>();
builder.Services.AddMediatR(configure => configure.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

builder.Services.AddExceptionHandler<ExceptionHandler.KnownExceptionsHandler>();

builder.Services.ConfigureFeatures(builder.Configuration, appAssembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProductionExceptionHandler();
app.MapCarter();
app.Run();

public partial class Program;
