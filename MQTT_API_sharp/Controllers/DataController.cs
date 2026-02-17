using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MQTT_API_sharp.Core.Entities;
using MQTT_API_sharp.Core.Interfaces;
using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Controllers
{
	[Route("api-mqtt/[controller]")]
	[ApiController]
	[Authorize]
	internal class DataController : ControllerBase
	{
		private readonly IDataRepository _dataRepository;

		public DataController(IDataRepository dataRepository, ILogger<DataController> logger)
		{
			_dataRepository = dataRepository;
		}

		[HttpPost("topics/add")]
		public async Task<IActionResult> AddTopicAsync([FromBody] CreateTopicDto topicDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
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

				return CreatedAtAction(nameof(GetTopicAsync), new { id = topic.ID_Topic }, topic);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error adding topic");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpDelete("topics/{topicId}")]
		public async Task<IActionResult> DeleteTopicAsync(int topicId)
		{
			try
			{
				var deletedCount = await _dataRepository.RemoveTopicAsync(topicId);

				if (deletedCount == 0)
					return NotFound($"Topic with ID {topicId} not found");

				return NoContent();
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error deleting topic");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("topics")]
		public async Task<IActionResult> GetTopicsAsync()
		{
			try
			{
				var topics = await _dataRepository.GetTopicsAsync();
				return Ok(topics);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error getting topics");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("topics/{topicId}")]
		public async Task<IActionResult> GetTopicAsync(int topicId)
		{
			if (topicId <= 0)
				return BadRequest("Valid topic ID is required");

			try
			{
				var topic = await _dataRepository.GetTopicAsync(topicId);

				if (topic == null)
					return NotFound();

				return Ok(topic);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error getting topic");
				return StatusCode(500, "Internal server error");
			}
		}
		
		[HttpGet("topics/formPath")]
		public async Task<IActionResult> GetTopicAsync([FromQuery] string? path = null)
		{
			if (string.IsNullOrWhiteSpace(path))
				return BadRequest("Valid topic path is required");

			try
			{
				var topic = await _dataRepository.GetTopicAsync(path);

				if (topic == null)
					return NotFound();

				return Ok(topic);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error getting topic");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("topics/{topicId}/data")]
		public async Task<IActionResult> GetTopicDataAsync(int topicId, [FromQuery] int? limit = null)
		{
			if (topicId <= 0)
				return BadRequest("Valid topic ID is required");

			try
			{
				List<Data> data;
				if (limit.HasValue && limit > 0)
					data = await _dataRepository.GetDataAsync(topicId, limit.Value);
				else
					data = await _dataRepository.GetDataAsync(topicId);

				if (data == null || data.Count == 0)
					return NotFound($"No data found for topic ID {topicId}");

				return Ok(data);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error getting topic data");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("topics/{topicId}/points")]
		public async Task<IActionResult> GetTopicPointsAsync(int topicId)
		{
			if (topicId <= 0)
				return BadRequest("Valid topic ID is required");

			try
			{
				AreaPoint? point = await _dataRepository.GetAreaPointsAsync(topicId);
				if (point == null)
					return NotFound($"No area points found for topic ID {topicId}");

				return Ok(point.Depression_AreaPoint);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error getting area points");
				return StatusCode(500, "Internal server error");
			}
		}
	}
}
