using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Shape.Command.CreateRectangle;
using Microsoft.AspNetCore.Mvc;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ShapeController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public ShapeController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpPost("rectangle")]
    public async Task<ActionResult<int>> CreateRectangle(CreateRectangleCommand command, CancellationToken token)
    {
        var result = await _dispatcher.Execute(command, token);
        return Ok(result);
    }
}