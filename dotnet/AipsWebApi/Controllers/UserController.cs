using AipsCore.Application.Abstract;
using AipsCore.Application.Models.User.Command.CreateUser;
using AipsCore.Application.Models.User.Query.GetUser;
using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public UserController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(userId);
        var result = await _dispatcher.Execute(query, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await _dispatcher.Execute(command, cancellationToken);
        return Ok(userId.IdValue);
    }
}