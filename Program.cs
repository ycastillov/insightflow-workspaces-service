using FluentValidation;
using FluentValidation.AspNetCore;
using InsightFlow.WorkspacesService.Src.DTOs;
using InsightFlow.WorkspacesService.Src.Interfaces;
using InsightFlow.WorkspacesService.Src.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddSingleton<IWorkspaceRepository, WorkspaceRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<CreateWorkspaceRequest>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateWorkspaceRequest>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
