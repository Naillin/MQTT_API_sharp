using MQTT_API_sharp.Core.Entities;
using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Services;

public class AuthService
{
    // public async Task<User?> LoginAsync(LoginDto loginModel)
    // {
    //     if (string.IsNullOrWhiteSpace(loginModel.Login) || string.IsNullOrWhiteSpace(loginModel.Password))
    //         return BadRequest("Error in auth!");
    //
    //     User? user = await _dataRepository.GetUserAsync(loginModel.Login);
	   //
    //     if (user == null)
    //         return BadRequest("User is not found!");
    //
    //     // Внимание: хранить пароли в открытом виде небезопасно, 
    //     // но оставляю логику сравнения как у вас в оригинале
    //     if (user.Password_User != loginModel.Password)
    //         return BadRequest("Wrong password!");
    //
    //     // === СОЗДАНИЕ СЕССИИ (COOKIE) ===
    //     var claims = new List<Claim>
    //     {
    //         new Claim(ClaimTypes.Name, user.Login_User),
    //         new Claim(ClaimTypes.NameIdentifier, user.ID_User.ToString())
    //         // Можно добавить роль, если есть: new Claim(ClaimTypes.Role, "Admin")
    //     };
    //
    //     var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    //     var authProperties = new AuthenticationProperties
    //     {
    //         IsPersistent = true, // Сохранять куку после закрытия браузера
    //         AllowRefresh = true
    //     };
    //
    //     await HttpContext.SignInAsync(
    //         CookieAuthenticationDefaults.AuthenticationScheme,
    //         new ClaimsPrincipal(claimsIdentity),
    //         authProperties);
    // }
}