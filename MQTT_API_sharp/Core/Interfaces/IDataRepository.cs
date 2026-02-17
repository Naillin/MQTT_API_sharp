using Microsoft.AspNetCore.Mvc;
using MQTT_API_sharp.Core.Entities;

namespace MQTT_API_sharp.Core.Interfaces
{
	internal interface IDataRepository
	{
		Task<User?> GetUserAsync(string login, CancellationToken cancellationToken = default);

		Task AddTopicAsync(Topic topic, CancellationToken cancellationToken = default);

		Task<int?> RemoveTopicAsync(int topicId, CancellationToken cancellationToken = default);

		Task<List<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default);

		Task<Topic?> GetTopicAsync(int id, CancellationToken cancellationToken = default);

		Task<Topic?> GetTopicAsync(string path, CancellationToken cancellationToken = default);

		Task<List<Data>> GetDataAsync(int topicId, CancellationToken cancellationToken = default);

		Task<List<Data>> GetDataAsync(int topicId, int limit, CancellationToken cancellationToken = default);

		Task<AreaPoint?> GetAreaPointsAsync(int topicId, CancellationToken cancellationToken = default);
	}
}
