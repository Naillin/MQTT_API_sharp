using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MQTT_API_sharp.Core.Models;

public class DataDto
{
    [Required]
    [JsonPropertyName("id_data")]
    public int ID_Data { get; set; }
    
    [Required(ErrorMessage = "Value is required")]
    [JsonPropertyName("value_data")]
    public string? Value_Data { get; set; }

    [Required(ErrorMessage = "Time is required")]
    [JsonPropertyName("time_data")]
    public long Time_Data { get; set; }
}