using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using Microsoft.OpenApi.Models;
using WebAPI.Dtos;
using WebAPI.Middlewares;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Enable globalization support
AppContext.SetSwitch("System.Globalization.Invariant", false);

// Register the DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

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

// Obtain the environment instance
var env = builder.Environment;  // This line is added to define 'env'

// Configure the HTTP request pipeline.
app.ConfigureExceptionHandler(env);

//app.ConfigureBuiltinExceptionHandler(env);

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
