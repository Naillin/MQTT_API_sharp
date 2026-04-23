using MQTT_API_sharp.Core.Models;
using WaterlevelSystem_DataBaseStructure.Entities;

namespace MQTT_API_sharp.Core.Interfaces;

public interface IDataService
{
    public Task<Topic> AddTopicAsync(CreateTopicDto topicDto);

    public Task<int> DeleteTopicAsync(int topicId);
    
    public Task<IList<TopicDto>> GetTopicsAsync();

    public Task<TopicDto?> GetTopicAsync(string? path = null);
    
    public Task<TopicDto?> GetTopicAsync(int topicId);

    public Task<IList<DataDto>> GetTopicDataAsync(int topicId, int? limit = null);

    public Task<string> GetTopicPointsAsync(int topicId);
}