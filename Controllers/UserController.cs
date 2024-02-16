using CajeroAPI.Models;
using CajeroAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sepalo.WebApi.Admin.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
     private readonly BaseRepository _baseRepository;
    private readonly IConfiguration _configuration;

    public UserController(BaseRepository baseRepository, IConfiguration configuration)
    {
        _baseRepository = baseRepository;
        _configuration = configuration;
    }

[HttpPost("login")]
public IActionResult Login(LoginModel model)
{
    OracleConnection _dbConnection = new OracleConnection("Data Source = localhost;User Id = system; Password = admin;");
    try
    {
        _dbConnection.Open();
        Console.WriteLine("Connection to database established successfully.");

        using var command = _dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM Users WHERE username = :username AND Password = :password";
        command.Parameters.Add(new OracleParameter("username", model.username));
        command.Parameters.Add(new OracleParameter("password", model.Password));

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            // Usuario encontrado
            var tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
            var key = Encoding.ASCII.GetBytes(tokenOptions.SecretKey);
            // Verificar que ExpirationMinutes es un valor positivo
            if (tokenOptions.ExpirationMinutes <= 0)
            {
            throw new Exception("ExpirationMinutes debe ser un valor positivo.");
            }

           var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, model.username)
        }),
            Expires = DateTime.UtcNow.AddMinutes(tokenOptions.ExpirationMinutes), 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
        else
        {
            // Usuario no encontrado
    string errorMessage = "Usuario no autorizado";
    return Content(errorMessage, "text/plain", System.Text.Encoding.UTF8);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error connecting to database: {ex.Message}");
        return StatusCode(500, "Error opening database connection");
    }
    finally
    {
        if (_dbConnection.State == ConnectionState.Open)
        {
            _dbConnection.Close();
             Console.WriteLine("Connection to database closed successfully.");
        }
    }
}

}