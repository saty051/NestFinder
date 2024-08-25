using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using Microsoft.OpenApi.Models;
using WebAPI.Dtos;
using WebAPI.Models;
using WebAPI.Middlewares;  // Import the namespace where your extension method is located

var builder = WebApplication.CreateBuilder(args);

// Enable globalization support
AppContext.SetSwitch("System.Globalization.Invariant", false);

// Register the DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Add UnitOfWork service
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure Swagger with custom schema for CityDto
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Custom example schema for CityDto
    c.MapType<CityDto>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["id"] = new OpenApiSchema { Type = "integer", Example = new Microsoft.OpenApi.Any.OpenApiInteger(0) },
            ["name"] = new OpenApiSchema { Type = "string", Example = new Microsoft.OpenApi.Any.OpenApiString("New York") },
            ["country"] = new OpenApiSchema { Type = "string", Example = new Microsoft.OpenApi.Any.OpenApiString("USA") }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Call the extension method to configure exception handling.
app.UseMiddleware<ExceptionMiddleware>();
app.ConfigureExceptionHandler(app.Environment);

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
