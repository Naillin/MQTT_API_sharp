using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MQTT_API_sharp.Core.Models;

public class PredictionDto
{
    [Required]
    [JsonPropertyName("id_prediction")]
    public int ID_Prediction { get; set; }
    
    [JsonPropertyName("value_prediction")]
    public string? Value_Prediction { get; set; }
    
    [JsonPropertyName("time_prediction")]
    public DateTimeOffset Time_Prediction { get; set; }
}