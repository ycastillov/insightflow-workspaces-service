using FluentValidation;
using FluentValidation.AspNetCore;
using InsightFlow.WorkspacesService.Src.DTOs;
using InsightFlow.WorkspacesService.Src.Interfaces;
using InsightFlow.WorkspacesService.Src.Repositories;
using InsightFlow.WorkspacesService.Src.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddSingleton<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateWorkspaceRequest>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateWorkspaceRequest>();

var corsSettings = builder.Configuration.GetSection("CorsSettings");
var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>();
var allowedMethods = corsSettings.GetSection("AllowedMethods").Get<string[]>();
var allowedHeaders = corsSettings.GetSection("AllowedHeaders").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "DefaultCorsPolicy",
        policy =>
        {
            policy
                .WithOrigins(allowedOrigins!)
                .WithHeaders(allowedHeaders!)
                .WithMethods(allowedMethods!)
                .AllowCredentials();
        }
    );
});

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("DefaultCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
