using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using Microsoft.OpenApi.Models;
using WebAPI.Dtos;
using WebAPI.Middlewares;
using WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Data.SqlClient;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
    .CreateLogger();

builder.Logging.AddSerilog();

// Add environment variables with the prefix "NESTFINDER"
builder.Configuration.AddEnvironmentVariables(prefix: "NESTFINDER_");

// Enable globalization support
AppContext.SetSwitch("System.Globalization.Invariant", false);

// Create a SqlConnectionStringBuilder using the connection string from configuration
var sqlBuilder = new SqlConnectionStringBuilder(
    builder.Configuration.GetConnectionString("DefaultConnection2"));

// Set the password using the environment variable or configuration setting
sqlBuilder.Password = builder.Configuration.GetSection("DBPassword").Value;

// Add this line to bypass SSL certificate validation
sqlBuilder.TrustServerCertificate = true;

// Get the final connection string with the password included
var connectionString = sqlBuilder.ConnectionString;

// Register the DbContext using the constructed connection string and set the CommandTimeout
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(300); // Set command timeout to 300 seconds (5 minutes)
    }));

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

// Get secret key from configuration
var secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = key
        };
    });

// Configure Swagger with JWT Authentication support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

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
var env = builder.Environment;

// Configure the HTTP request pipeline.
app.ConfigureExceptionHandler(env);

if (env.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
