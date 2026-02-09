using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;
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
}