using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Whiteboard.Command.AddUserToWhiteboard;
using AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;
using AipsCore.Application.Models.Whiteboard.Query.GetRecentWhiteboards;
using AipsCore.Domain.Models.Whiteboard;
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
    
    [HttpPost]
    public async Task<ActionResult<int>> CreateWhiteboard(CreateWhiteboardCommand command, CancellationToken cancellationToken)
    {
        var whiteboardId = await _dispatcher.Execute(command, cancellationToken);
        return Ok(whiteboardId.IdValue);
    }

    [HttpPost("adduser")]
    public async Task<IActionResult> AddUser(AddUserToWhiteboardCommand command,
        CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(command, cancellationToken);
        return Ok();
    }

    [HttpGet("recent")]
    public async Task<ActionResult<ICollection<Whiteboard>>> Recent(GetRecentWhiteboardsQuery query, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(query, cancellationToken);

        return Ok(result);
    }
}