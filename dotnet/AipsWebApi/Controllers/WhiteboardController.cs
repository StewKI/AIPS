using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.DeleteWhiteboard;
using AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;
using AipsCore.Application.Models.Whiteboard.Query.GetWhiteboard;
using AipsCore.Application.Models.Whiteboard.Query.GetWhiteboardHistory;
using AipsCore.Application.Models.WhiteboardMembership.Command.CreateWhiteboardMembership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Whiteboard = AipsCore.Infrastructure.Persistence.Whiteboard.Whiteboard;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WhiteboardController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public WhiteboardController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<string>> CreateWhiteboard(CreateWhiteboardCommand command, CancellationToken cancellationToken)
    {
        var whiteboardId = await _dispatcher.Execute(command, cancellationToken);
        return Ok(whiteboardId.IdValue);
    }
    
    [Authorize]
    [HttpGet("{whiteboardId}")]
    public async Task<ActionResult<Whiteboard>> GetWhiteboardById([FromRoute] string whiteboardId, CancellationToken cancellationToken)
    {
        var whiteboard = await _dispatcher.Execute(new GetWhiteboardQuery(whiteboardId), cancellationToken);
        if (whiteboard == null)
        {
            return NotFound();
        }
        return Ok(whiteboard);
    }

    [Authorize]
    [HttpDelete("{whiteboardId}")]
    public async Task<ActionResult> DeleteWhiteboard([FromRoute] string whiteboardId, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(new DeleteWhiteboardCommand(whiteboardId), cancellationToken);
        return Ok();
    }


    [Authorize]
    [HttpGet("history")]
    public async Task<ActionResult<ICollection<Whiteboard>>> GetWhiteboardHistory(CancellationToken cancellationToken)
    {
        var whiteboards = await _dispatcher.Execute(new GetWhiteboardHistoryQuery(), cancellationToken);
        return Ok(whiteboards);
    }

    [Authorize]
    [HttpGet("recent")]
    public async Task<ActionResult<ICollection<Whiteboard>>> GetRecentWhiteboards(CancellationToken cancellationToken)
    {
        var whiteboards = await _dispatcher.Execute(new GetRecentWhiteboardsQuery(), cancellationToken);
        return Ok(whiteboards);
    }

    [Authorize]
    [HttpPost("join")]
    public async Task<ActionResult> JoinWhiteboard(CreateWhiteboardMembershipCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result);
    }
}