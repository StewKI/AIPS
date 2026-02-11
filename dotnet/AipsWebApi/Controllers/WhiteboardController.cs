using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Whiteboard.Command.BanUserFromWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.KickUserFromWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.UnbanUserFromWhiteboard;
using Microsoft.AspNetCore.Mvc;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WhiteboardController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> CreateWhiteboard(CreateWhiteboardCommand command, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var whiteboardId = await dispatcher.Execute(command, cancellationToken);
        return Ok(whiteboardId.IdValue);
    }
    
    [HttpPut("banUser")]
    public async Task<ActionResult> BanUserFromWhiteboard(BanUserFromWhiteboardCommand command, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        await dispatcher.Execute(command, cancellationToken);
        return Ok();
    }
    
    [HttpPut("unbanUser")]
    public async Task<ActionResult> UnbanUserFromWhiteboard(UnbanUserFromWhiteboardCommand command, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        await dispatcher.Execute(command, cancellationToken);
        return Ok();
    }
    
    [HttpPut("kickUser")]
    public async Task<ActionResult> KickUserFromWhiteboard(KickUserFromWhiteboardCommand command, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        await dispatcher.Execute(command, cancellationToken);
        return Ok();
    }
}