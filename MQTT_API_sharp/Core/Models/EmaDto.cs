using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MQTT_API_sharp.Core.Models;

public class EmaDto
{
    [Required]
    [JsonPropertyName("id_ema")]
    public int ID_Ema { get; set; }
    
    [JsonPropertyName("value_ema")]
    public string? Value_Ema { get; set; }
    
    [JsonPropertyName("time_ema")]
    public DateTimeOffset Time_Ema { get; set; }
}