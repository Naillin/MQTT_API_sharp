using System.Security.Claims;
using MQTT_API_sharp.Core.Entities;
using MQTT_API_sharp.Core.Exceptions;
using MQTT_API_sharp.Core.Interfaces;
using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Services;

public class AuthService : IAuthService
{
    private readonly IDataRepository _dataRepository;
    private readonly ILogger<IAuthService> _logger;
    
    public AuthService(IDataRepository dataRepository, ILogger<IAuthService> logger)
    {
        _dataRepository = dataRepository;
        _logger = logger;
    }
    
    public async Task<IList<Claim>> LoginAsync(LoginDto loginModel)
    {
        _logger.LogDebug($"Trying to login for user: {loginModel.Login}");
        
        if (string.IsNullOrWhiteSpace(loginModel.Login) || string.IsNullOrWhiteSpace(loginModel.Password))
            throw new ArgumentException("Login and password are required");
        
        User? user = await _dataRepository.GetUserAsync(loginModel.Login);
        
        // Внимание: хранить пароли в открытом виде небезопасно
        if (user == null || user.Password_User != loginModel.Password)
        {
            _logger.LogWarning($"Failed login attempt for user: {loginModel.Login}");
            throw new AuthException("Invalid login or password"); 
        }
        
        // === СОЗДАНИЕ СЕССИИ (COOKIE) ===
        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login_User),
            new Claim(ClaimTypes.NameIdentifier, user.ID_User.ToString())
            // Можно добавить роль, если есть: new Claim(ClaimTypes.Role, "Admin")
        };
    }
}