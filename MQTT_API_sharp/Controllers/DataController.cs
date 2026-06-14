using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MQTT_API_sharp.Core.Interfaces;
using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Controllers;

[Route("api-mqtt/[controller]")]
[ApiController]
[Authorize]
public class DataController : ControllerBase
{
	private readonly IDataService _dataService;
	private readonly ILogger<DataController> _logger;

	public DataController(IDataService dataService, ILogger<DataController> logger)
	{
		_dataService = dataService;
		_logger = logger;
	}

	[HttpPost("topics/add")]
	public async Task<IActionResult> AddTopicAsync([FromBody] CreateTopicDto topicDto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		var topic = await _dataService.AddTopicAsync(topicDto);
		return CreatedAtRoute("GetTopicById", new { topicId = topic.ID_Topic }, topic);
	}

	[HttpDelete("topics/{topicId}")]
	public async Task<IActionResult> DeleteTopicAsync(int topicId)
	{
		await _dataService.DeleteTopicAsync(topicId);
		return NoContent();
	}

	[HttpGet("topics")]
	public async Task<IActionResult> GetTopicsAsync()
	{
		var topics = await _dataService.GetTopicsAsync();
		return Ok(topics);
	}

	[HttpGet("topics/fromPath")]
	public async Task<IActionResult> GetTopicAsync([FromQuery] string? path = null)
	{
		var topic = await _dataService.GetTopicAsync(path);
		return Ok(topic);
	}
		
	[HttpGet("topics/{topicId}", Name = "GetTopicById")]
	public async Task<IActionResult> GetTopicAsync(int topicId)
	{
		var topic = await _dataService.GetTopicAsync(topicId);
		return Ok(topic);
	}

	[HttpGet("topics/{topicId}/data")]
	public async Task<IActionResult> GetTopicDataAsync(int topicId, [FromQuery] int? limit = null)
	{
		var data = await _dataService.GetTopicDataAsync(topicId, limit);
		return Ok(data);
	}
	
	[HttpGet("topics/{topicId}/ema")]
	public async Task<IActionResult> GetTopicEmaAsync(int topicId, [FromQuery] int? limit = null)
	{
		var data = await _dataService.GetTopicEmaAsync(topicId, limit);
		return Ok(data);
	}
	
	[HttpGet("topics/{topicId}/prediction")]
	public async Task<IActionResult> GetTopicPredictionAsync(int topicId)
	{
		var data = await _dataService.GetTopicPredictionAsync(topicId);
		return Ok(data);
	}

	[HttpGet("topics/{topicId}/points")]
	public async Task<IActionResult> GetTopicPointsAsync(int topicId)
	{
		var points = await _dataService.GetTopicPointsAsync(topicId);
		return Ok(points);
	}
}