using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TodoAppTakeHome.Api.Data;
using TodoAppTakeHome.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// EF Core with SQLite
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=todoapp.db"));

// Add controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoAppTakeHome API", Version = "v1" });
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Register TaskService for DI
builder.Services.AddScoped<ITaskService, TaskService>();

// Build the application
var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoAppTakeHome API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
