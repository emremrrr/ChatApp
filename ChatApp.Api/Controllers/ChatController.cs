using ChatApp.Domain.Models;
using ChatApp.Queue.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController(RabbitMqSessionManager sessionManager,CoordinatorService coordinatorService): ControllerBase
{

    [HttpPost("create/{clientId}")]
    public IActionResult StartSession(Guid clientId)
    {
        try
        {
            var agentSessionId= coordinatorService.AddAgentToQueue();
            var res=sessionManager.CreateSession(agentSessionId);
            return Ok($"Session started for client ID: {agentSessionId}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Poll/{clientId}")]
    public IActionResult Poll(Guid clientId){
        try {
           return Ok(sessionManager.CheckSession(clientId));
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

}

