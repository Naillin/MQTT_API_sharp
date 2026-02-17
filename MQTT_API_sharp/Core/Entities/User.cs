using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MQTT_API_sharp.Core.Entities
{
	[Table("users")]
	internal class User
	{
		[Key]
		[Column("id_user")]
		public int ID_User { get; set; }

		[MaxLength(100)]
		[MinLength(3)]
		[Required]
		[Column("login_user")]
		public string? Login_User { get; set; }

		[MaxLength(100)]
		[MinLength(3)]
		[Required]
		[Column("password_user")]
		public string? Password_User { get; set; }
	}
}
