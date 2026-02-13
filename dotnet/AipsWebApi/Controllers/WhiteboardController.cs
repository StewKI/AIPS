using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Whiteboard.Command.AddUserToWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.BanUserFromWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.KickUserFromWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.UnbanUserFromWhiteboard;
using AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;
using AipsCore.Domain.Models.Whiteboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WhiteboardController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public WhiteboardController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<int>> CreateWhiteboard(CreateWhiteboardCommand command, CancellationToken cancellationToken)
    {
        var whiteboardId = await _dispatcher.Execute(command, cancellationToken);
        return Ok(whiteboardId.IdValue);
    }
}