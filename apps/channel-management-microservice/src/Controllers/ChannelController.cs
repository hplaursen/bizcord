using Microsoft.AspNetCore.Mvc;
using ChannelManagement.Services;
using ChannelManagement.Shared.Models;
using ChannelManagement.Domain.Entities;

namespace ChannelManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelController(IChannelService channelService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChannelById(int id)
    {
        Channel? channel = 
            await channelService.GetChannelByIdAsync(id);
        if (channel == null)
            return NotFound();

        return Ok(channel);
    }

    [HttpGet]
    public async Task<IActionResult> GetChannels()
    {
        IEnumerable<Channel>? channels = 
            await channelService.GetAllChannelsAsync();
        if (channels == null)
            return NotFound();

        return Ok(channels);
    }


    [HttpPost]
    public async Task<IActionResult> CreateChannel([FromBody] ChannelDto channelDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdChannel = await channelService.CreateChannelAsync(MapToChannel(channelDto));
        return CreatedAtAction(nameof(GetChannelById), new { id = createdChannel.Id }, createdChannel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChannel(int id, [FromBody] ChannelDto channelDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedChannel = await channelService.UpdateChannelAsync(id, MapToChannel(channelDto));
        if (updatedChannel == null)
        {
            return NotFound();
        }
        return Ok(updatedChannel);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChannel(int id)
    {
        var result = await channelService.DeleteChannelAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }


    private Channel MapToChannel(ChannelDto channelDto)
    {
        return new Channel
        {
            Id = channelDto.Id,
            Name = channelDto.Name,
            Description = channelDto.Description,
            CreatedAt = channelDto.CreatedAt
        };
    }
}