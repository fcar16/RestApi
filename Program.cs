using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sepalo.WebApi.Admin.Repository;
﻿using System.Collections;
using CajeroAPI.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;


var builder = WebApplication.CreateBuilder(args);
string repositoryName = "api";
builder.Services.AddScoped<BaseRepository>(serviceProvider => 
    new BaseRepository(repositoryName, serviceProvider.GetRequiredService<IConfiguration>()));

// Configure JWT authentication
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
var key = Encoding.ASCII.GetBytes(tokenOptions.SecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Open database connection

 OracleConnection _dbConnection = new OracleConnection("Data Source = localhost;User Id = system; Password = admin;");
try
{
    _dbConnection.Open();
    Console.WriteLine("Connection to database established successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to database: {ex.Message}");
}
 finally
    {
        if (_dbConnection.State == ConnectionState.Open)
        {
            _dbConnection.Close();
            Console.WriteLine("Connection to database closed successfully.");
        }
    }



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options => options
    .AllowAnyOrigin() 
    .AllowAnyMethod() 
    .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
