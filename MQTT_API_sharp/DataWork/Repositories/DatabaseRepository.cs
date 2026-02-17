using Microsoft.EntityFrameworkCore;
using MQTT_API_sharp.Core.Entities;
using MQTT_API_sharp.Core.Interfaces;

namespace MQTT_API_sharp.DataWork.Repositories
{
	internal class DatabaseRepository : IDataRepository
	{
		private readonly IDbContextFactory<AppDbContext> _factory;

		public DatabaseRepository(IDbContextFactory<AppDbContext> factory) => _factory = factory;

		public async Task<User?> GetUserAsync(string login, CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			return await db.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Login_User == login, cancellationToken);
		}

		public async Task AddTopicAsync(Topic topic, CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			await db.Topics.AddAsync(topic, cancellationToken);
			await db.SaveChangesAsync(cancellationToken);
		}

		public async Task<int?> RemoveTopicAsync(int topicId, CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			return await db.Topics
				.Where(t => t.ID_Topic == topicId)
				.ExecuteDeleteAsync(cancellationToken);
		}

		public async Task<List<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			return await db.Topics
				.AsNoTracking()
				.ToListAsync(cancellationToken);
		}

		public async Task<Topic?> GetTopicAsync(int topicId, CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			return await db.Topics
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.ID_Topic == topicId, cancellationToken);
		}

		public async Task<Topic?> GetTopicAsync(string path, CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			return await db.Topics
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.Path_Topic == path, cancellationToken);
		}

		public async Task<List<Data>> GetDataAsync(int topicId, CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			return await db.Data
				.Where(d => d.ID_Topic == topicId)
				.OrderByDescending(d => d.Time_Data)
				.ToListAsync(cancellationToken);
		}

		public async Task<List<Data>> GetDataAsync(int topicId, int limit, CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			return await db.Data
				.Where(d => d.ID_Topic == topicId)
				.OrderByDescending(d => d.Time_Data)
				.Take(limit)
				.ToListAsync(cancellationToken);
		}

		public async Task<AreaPoint?> GetAreaPointsAsync(int topicId, CancellationToken cancellationToken = default)
		{
			await using var db = await _factory.CreateDbContextAsync(cancellationToken);
			return await db.AreaPoints
				.AsNoTracking()
				.FirstOrDefaultAsync(d => d.ID_Topic == topicId, cancellationToken);
		}
	}
}
