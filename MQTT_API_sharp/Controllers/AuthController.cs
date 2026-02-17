using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MQTT_API_sharp.Core.Entities;
using MQTT_API_sharp.Core.Interfaces;
using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Controllers
{
	[Route("api-mqtt/[controller]")]
	[ApiController]
	internal class AuthController : ControllerBase
	{
		private readonly IDataRepository _dataRepository;

		public AuthController(IDataRepository dataRepository, ILogger<AuthController> logger)
		{
			_dataRepository = dataRepository;
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
				Login = User.Identity?.Name, // Login, который мы записали в ClaimTypes.Name
				Id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value // ID пользователя
			});
		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginModel)
		{
			if (string.IsNullOrWhiteSpace(loginModel.Login) || string.IsNullOrWhiteSpace(loginModel.Password))
				return BadRequest("Error in auth!");

			User? user = await _dataRepository.GetUserAsync(loginModel.Login);
    
			if (user == null)
				return BadRequest("User is not found!");

			// Внимание: хранить пароли в открытом виде небезопасно, 
			// но оставляю логику сравнения как у вас в оригинале
			if (user.Password_User != loginModel.Password)
				return BadRequest("Wrong password!");

			// === СОЗДАНИЕ СЕССИИ (COOKIE) ===
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Login_User),
				new Claim(ClaimTypes.NameIdentifier, user.ID_User.ToString())
				// Можно добавить роль, если есть: new Claim(ClaimTypes.Role, "Admin")
			};

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

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> LogoutAsync()
		{
			// === УДАЛЕНИЕ СЕССИИ ===
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok(new { message = "Logged out" });
		}
	}
}
