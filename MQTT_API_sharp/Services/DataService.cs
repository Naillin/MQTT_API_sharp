using MQTT_API_sharp.Core.Entities;
using MQTT_API_sharp.Core.Interfaces;
using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Services;

public class DataService : IDataService
{
    private readonly IDataRepository _dataRepository;
    private readonly ILogger<DataService> _logger;
    
    public DataService(IDataRepository dataRepository, ILogger<DataService> logger)
    {
        _dataRepository = dataRepository;
        _logger = logger;
    }

    public async Task<Topic> AddTopicAsync(CreateTopicDto topicDto)
    {
        // Маппим DTO в entity
        var topic = new Topic
        {
            Name_Topic = topicDto.Name_Topic,
            Path_Topic = topicDto.Path_Topic,
            Latitude_Topic = topicDto.Latitude_Topic,
            Longitude_Topic = topicDto.Longitude_Topic,
            Altitude_Topic = topicDto.Altitude_Topic,
            AltitudeSensor_Topic = topicDto.AltitudeSensor_Topic,
            //CheckTime_Topic = DateTime.UtcNow.Ticks
        };

        await _dataRepository.AddTopicAsync(topic);
        
        return topic;
    }

    public async Task<int> DeleteTopicAsync(int topicId)
    {
        var deletedCount = await _dataRepository.RemoveTopicAsync(topicId);
        
        if (deletedCount == 0)
            throw new KeyNotFoundException($"Topic with ID {topicId} not found");
        
        return deletedCount;
    }

    public async Task<IList<Topic>> GetTopicsAsync() => await _dataRepository.GetTopicsAsync();

    public async Task<Topic?> GetTopicAsync(int topicId)
    {
        TopicIdOutOfRangeCheck(topicId);

        var topic = await _dataRepository.GetTopicAsync(topicId);

        if (topic == null)
            throw new KeyNotFoundException($"Topic with ID {topicId} not found");

        return topic;
    }
    
    public async Task<Topic?> GetTopicAsync(string? path = null)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentNullException(nameof(path), "Valid topic path is required");

        var topic = await _dataRepository.GetTopicAsync(path);

        if (topic == null)
            throw new KeyNotFoundException($"Topic with topic path {path} not found");

        return topic;
    }

    public async Task<IList<Data>> GetTopicDataAsync(int topicId, int? limit = null)
    {
        TopicIdOutOfRangeCheck(topicId);
        
        IList<Data> data = limit.HasValue && limit > 0
                ? await _dataRepository.GetDataAsync(topicId, limit.Value)
                : await _dataRepository.GetDataAsync(topicId);

        if (data == null || data.Count == 0)
            return new List<Data>();
        
        return data;
    }

    public async Task<string> GetTopicPointsAsync(int topicId)
    {
        TopicIdOutOfRangeCheck(topicId);
        
        AreaPoint? point = await _dataRepository.GetAreaPointsAsync(topicId);
        
        if (point == null || string.IsNullOrWhiteSpace(point.Depression_AreaPoint))
            return string.Empty;

        return point.Depression_AreaPoint;
    }

    private void TopicIdOutOfRangeCheck(int topicId)
    {
        if (topicId <= 0)
            throw new ArgumentOutOfRangeException(nameof(topicId), "Valid topic ID is required");
    }
}