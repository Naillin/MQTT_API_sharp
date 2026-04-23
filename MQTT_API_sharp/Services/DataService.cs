using MQTT_API_sharp.Core.Interfaces;
using MQTT_API_sharp.Core.Models;
using WaterlevelSystem_DataBaseStructure.Entities;

namespace MQTT_API_sharp.Services;

public class DataService : IDataService
{
    private readonly IDataRepository _dataRepository;
    private readonly ILogger<IDataService> _logger;
    
    public DataService(IDataRepository dataRepository, ILogger<IDataService> logger)
    {
        _dataRepository = dataRepository;
        _logger = logger;
    }

    public async Task<TopicDto> AddTopicAsync(CreateTopicDto topicDto)
    {
        _logger.LogDebug($"Adding topic with path: {topicDto.Path_Topic}");

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
        
        return new TopicDto
        {
            ID_Topic = topic.ID_Topic,
            Name_Topic = topic.Name_Topic,
            Path_Topic = topic.Path_Topic,
            Latitude_Topic = topic.Latitude_Topic,
            Longitude_Topic = topic.Longitude_Topic,
            Altitude_Topic = topic.Altitude_Topic,
            AltitudeSensor_Topic = topic.AltitudeSensor_Topic
        };
    }

    public async Task<int> DeleteTopicAsync(int topicId)
    {
        _logger.LogDebug($"Remove topic with id: {topicId}");
        
        TopicIdOutOfRangeCheck(topicId);
        
        var deletedCount = await _dataRepository.RemoveTopicAsync(topicId);
        
        if (deletedCount == 0)
            throw new KeyNotFoundException($"Topic with ID {topicId} not found");
        
        return deletedCount;
    }

    public async Task<IList<TopicDto>> GetTopicsAsync() => (await _dataRepository.GetTopicsAsync())
        .Select(topic => new TopicDto
        {
            ID_Topic =  topic.ID_Topic,
            Name_Topic = topic.Name_Topic,
            Path_Topic = topic.Path_Topic,
            Latitude_Topic = topic.Latitude_Topic,
            Longitude_Topic = topic.Longitude_Topic,
            Altitude_Topic = topic.Altitude_Topic,
            AltitudeSensor_Topic = topic.AltitudeSensor_Topic
        }).ToList();
    
    public async Task<TopicDto?> GetTopicAsync(string? path = null)
    {
        _logger.LogDebug($"Getting topic with path: {path}");
        
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException(nameof(path), "Valid topic path is required");

        var topic = await _dataRepository.GetTopicAsync(path);

        if (topic == null)
            throw new KeyNotFoundException($"Topic with topic path {path} not found");

        return new()
        {
            ID_Topic =  topic.ID_Topic,
            Name_Topic = topic.Name_Topic,
            Path_Topic = topic.Path_Topic,
            Latitude_Topic = topic.Latitude_Topic,
            Longitude_Topic = topic.Longitude_Topic,
            Altitude_Topic = topic.Altitude_Topic,
            AltitudeSensor_Topic = topic.AltitudeSensor_Topic
        };
    }
    
    public async Task<TopicDto?> GetTopicAsync(int topicId)
    {
        _logger.LogDebug($"Getting topic with id: {topicId}");
        
        TopicIdOutOfRangeCheck(topicId);

        var topic = await _dataRepository.GetTopicAsync(topicId);

        if (topic == null)
            throw new KeyNotFoundException($"Topic with ID {topicId} not found");

        return new()
        {
            ID_Topic =  topic.ID_Topic,
            Name_Topic = topic.Name_Topic,
            Path_Topic = topic.Path_Topic,
            Latitude_Topic = topic.Latitude_Topic,
            Longitude_Topic = topic.Longitude_Topic,
            Altitude_Topic = topic.Altitude_Topic,
            AltitudeSensor_Topic = topic.AltitudeSensor_Topic
        };
    }
    
    public async Task<IList<DataDto>> GetTopicDataAsync(int topicId, int? limit = null)
    {
        _logger.LogDebug($"Getting topic data with id: {topicId}; And limit: {limit ?? -1}");
        
        TopicIdOutOfRangeCheck(topicId);
        
        IList<Data> dataPack = limit.HasValue && limit > 0
                ? await _dataRepository.GetDataAsync(topicId, limit.Value)
                : await _dataRepository.GetDataAsync(topicId);

        if (dataPack.Count == 0)
            return new List<DataDto>();
        
        return dataPack.Select(data => new DataDto
        {
            ID_Data =  data.ID_Data,
            Value_Data = data.Value_Data,
            Time_Data = data.Time_Data
        }).ToList();
    }

    public async Task<IList<EmaDto>> GetTopicEmaAsync(int topicId, int? limit = null)
    {
        _logger.LogDebug($"Getting topic ema with id: {topicId}; And limit: {limit ?? -1}");
        
        TopicIdOutOfRangeCheck(topicId);
        
        IList<Ema> emaPack = limit.HasValue && limit > 0
            ? await _dataRepository.GetEmaAsync(topicId, limit.Value)
            : await _dataRepository.GetEmaAsync(topicId);

        if (emaPack.Count == 0)
            return new List<EmaDto>();
        
        return emaPack.Select(ema => new EmaDto
        {
            ID_Ema = ema.ID_Ema,
            Value_Ema = ema.Value_Ema,
            Time_Ema = ema.Time_Ema
        }).ToList();
    }

    public async Task<IList<PredictionDto>> GetTopicPredictionAsync(int topicId)
    {
        _logger.LogDebug($"Getting topic prediction with id: {topicId}");
        
        TopicIdOutOfRangeCheck(topicId);
        
        IList<Prediction> predictionPack = await _dataRepository.GetPredictionsAsync(topicId);

        if (predictionPack.Count == 0)
            return new List<PredictionDto>();
        
        return predictionPack.Select(prediction => new PredictionDto
        {
            ID_Prediction = prediction.ID_Prediction,
            Value_Prediction = prediction.Value_Prediction,
            Time_Prediction = prediction.Time_Prediction
        }).ToList();
    }

    public async Task<string> GetTopicPointsAsync(int topicId)
    {
        _logger.LogDebug($"Getting topic area points with id: {topicId}");
        
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