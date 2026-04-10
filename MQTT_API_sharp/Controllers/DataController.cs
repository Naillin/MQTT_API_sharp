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
	public class DataController : ControllerBase
	{
		private readonly IDataService _dataService;
		private readonly ILogger<IDataRepository> _logger;

		public DataController(IDataService dataService, ILogger<IDataRepository> logger)
		{
			_dataService = dataService;
			_logger = logger;
		}

		[HttpPost("topics/add")]
		public async Task<IActionResult> AddTopicAsync([FromBody] CreateTopicDto topicDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _dataService.AddTopicAsync(topicDto);
				return CreatedAtAction(nameof(GetTopicAsync), new { id = result.ID_Topic }, result);
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
				await _dataService.DeleteTopicAsync(topicId);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
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
				var topics = await _dataService.GetTopicsAsync();
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
			try
			{
				var topic = await _dataService.GetTopicAsync(topicId);
				return Ok(topic);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
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
			try
			{
				var topic = await _dataService.GetTopicAsync(path);
				return Ok(topic);
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
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
			try
			{
				var data = await _dataService.GetTopicDataAsync(topicId, limit);
				return Ok(data);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				return BadRequest(ex.Message);
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
			try
			{
				var points = await _dataService.GetTopicPointsAsync(topicId);
				return Ok(points);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				//_logger.LogError(ex, "Error getting area points");
				return StatusCode(500, "Internal server error");
			}
		}
	}
}
