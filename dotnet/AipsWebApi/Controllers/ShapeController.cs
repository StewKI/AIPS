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
}