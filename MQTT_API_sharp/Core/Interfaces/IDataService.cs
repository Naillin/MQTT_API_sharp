using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Core.Interfaces;

public interface IDataService
{
    Task<TopicDto> AddTopicAsync(CreateTopicDto topicDto);

    Task<int> DeleteTopicAsync(int topicId);
    
    Task<IList<TopicDto>> GetTopicsAsync();

    Task<TopicDto?> GetTopicAsync(string? path = null);
    
    Task<TopicDto?> GetTopicAsync(int topicId);

    Task<IList<DataDto>> GetTopicDataAsync(int topicId, int? limit = null);
    
    Task<IList<EmaDto>> GetTopicEmaAsync(int topicId, int? limit = null);
    
    Task<IList<PredictionDto>> GetTopicPredictionAsync(int topicId);

    Task<string> GetTopicPointsAsync(int topicId);
}