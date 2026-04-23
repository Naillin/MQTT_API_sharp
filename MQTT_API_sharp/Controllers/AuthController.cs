using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MQTT_API_sharp.Core.Exceptions;
using MQTT_API_sharp.Core.Interfaces;
using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Controllers;

[Route("api-mqtt/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly ILogger<AuthController> _logger;

	public AuthController(IAuthService authService, ILogger<AuthController> logger)
	{
		_authService = authService;
		_logger = logger;
	}

	[Authorize]
	[HttpGet("check-auth")]
	public IActionResult CheckAuth()
	{
		// Если код дошел сюда, значит кука валидна (спасибо атрибуту [Authorize]).
		// Данные пользователя берем из User (ClaimsPrincipal).
	
		return Ok(new
		{
			IsAuthenticated = true,
			Login = User.Identity?.Name, // Login, который записали в ClaimTypes.Name
			Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value // ID пользователя
		});
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginModel)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
			
		try
		{
			var claims = await _authService.LoginAsync(loginModel);
				
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var authProperties = new AuthenticationProperties
			{
				IsPersistent = true, // Сохранять куку после закрытия браузера
				AllowRefresh = true
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);

			return Ok(new { message = "Logged in successfully" });
		}
		catch (ArgumentException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (AuthException ex)
		{
			return Unauthorized(new { message = ex.Message });
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting topic");
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[Authorize]
	[HttpPost("logout")]
	public async Task<IActionResult> LogoutAsync()
	{
		// === УДАЛЕНИЕ СЕССИИ ===
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		return Ok(new { message = "Logged out" });
	}
}