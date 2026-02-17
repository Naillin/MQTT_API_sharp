using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MQTT_API_sharp.Core.Models
{
	internal class LoginDto
	{
		[Required(ErrorMessage = "Login is required")]
		[JsonPropertyName("login_user")]
		public string? Login { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[JsonPropertyName("password_user")]
		public string? Password { get; set; }
	}
}
