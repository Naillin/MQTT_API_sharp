using System.ComponentModel.DataAnnotations;

namespace MQTT_API_sharp.Core.Models
{
	internal class CreateTopicDto
	{
		[MinLength(1)]
		[Required]
		public string? Name_Topic { get; set; }

		[MinLength(1)]
		[Required]
		public string? Path_Topic { get; set; }

		[Range(-90, 90)]
		public double Latitude_Topic { get; set; }

		[Range(-180, 180)]
		public double Longitude_Topic { get; set; }

		public double Altitude_Topic { get; set; }

		public double AltitudeSensor_Topic { get; set; }
	}
}
