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
using System.Security.Cryptography;
using System;




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
    // Obtén el hash de la contraseña proporcionada por el usuario
    string providedPasswordHash;
    using (SHA256 sha256Hash = SHA256.Create())
    {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        providedPasswordHash = builder.ToString();
    }

    OracleConnection _dbConnection = new OracleConnection("Data Source = localhost;User Id = system; Password = admin;");
    try
    {
        _dbConnection.Open();
        Console.WriteLine("Connection to database established successfully.");

        using var command = _dbConnection.CreateCommand();
        command.CommandText = "SELECT username, Password, profile_id  FROM Users WHERE username = :username AND Password = :password";
        command.Parameters.Add(new OracleParameter("username", model.username));
        command.Parameters.Add(new OracleParameter("password", providedPasswordHash));
       

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

[HttpPost("register")]
public IActionResult Register(LoginModel model)
{

  
    if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.Password) || model.Profile_Id == 0)
    {
        return BadRequest("Por favor, rellene todos los campos.");
    }
    using (SHA256 sha256Hash = SHA256.Create())
{
    // Convertir la entrada en un array de bytes y calcular el hash
    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

    // Convertir el array de bytes en una cadena hexadecimal
    StringBuilder builder = new StringBuilder();
    for (int i = 0; i < bytes.Length; i++)
    {
        builder.Append(bytes[i].ToString("x2"));
    }
    model.Password = builder.ToString();
    Console.WriteLine("Password: " + model.Password);
}

    
    OracleConnection _dbConnection = new OracleConnection("Data Source = localhost;User Id = system; Password = admin;");
    try
    {
        _dbConnection.Open();
        Console.WriteLine("Connection to database established successfully.");

        using var command = _dbConnection.CreateCommand();
        command.CommandText = "INSERT INTO Users (username, Password, profile_id) VALUES (:username, :password, :profile_id)";
        command.Parameters.Add(new OracleParameter("username", model.username));
        command.Parameters.Add(new OracleParameter("password", model.Password));
        command.Parameters.Add(new OracleParameter("profile_id", model.Profile_Id));

        command.ExecuteNonQuery();

       return Ok(new { message = "User registered successfully" });
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