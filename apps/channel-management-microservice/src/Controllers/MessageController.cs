
using Microsoft.AspNetCore.Mvc;
using ChannelManagement.Infrastructure;

namespace ChannelManagement.Controllers;


[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    
    private readonly IMessageClient _messageClient;
    public MessageController(IMessageClient messageClient)
    {
        _messageClient = messageClient;
    }

    [HttpPost]
    public async Task Post(string message)
    {
        await _messageClient.PublishAsync(message);
    }



}

