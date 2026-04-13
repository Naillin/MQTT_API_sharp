using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MQTT_API_sharp.Core.Models
{
	public class TopicDto
	{
		[Required]
		[JsonPropertyName("id_topic")]
		public int ID_Topic { get; set; }
		
		[MinLength(1)]
		[Required]
		[JsonPropertyName("name_topic")]
		public string? Name_Topic { get; set; }

		[MinLength(1)]
		[Required]
		[JsonPropertyName("path_topic")]
		public string? Path_Topic { get; set; }

		[Required]
		[Range(-90.0, 90.0)]
		[JsonPropertyName("latitude_topic")]
		public double Latitude_Topic { get; set; }

		[Required]
		[Range(-180.0, 180.0)]
		[JsonPropertyName("longitude_topic")]
		public double Longitude_Topic { get; set; }

		[JsonPropertyName("altitude_topic")]
		public double Altitude_Topic { get; set; }

		[JsonPropertyName("altitudeSensor_topic")]
		public double AltitudeSensor_Topic { get; set; }
	}
}
