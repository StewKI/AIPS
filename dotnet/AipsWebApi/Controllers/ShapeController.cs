using AipsCore.Application.Abstract;
using AipsCore.Application.Models.Shape.Command.CreateArrow;
using AipsCore.Application.Models.Shape.Command.CreateLine;
using AipsCore.Application.Models.Shape.Command.CreateTextShape;
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

    [HttpPost("arrow")]
    public async Task<IActionResult> CreateArrow(CreateArrowCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("textShape")]
    public async Task<IActionResult> CreateTextShape(CreateTextShapeCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("line")]
    public async Task<IActionResult> CreateLine(CreateLineCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result);
    }
}