using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Models.User.Command.LogIn;
using AipsCore.Application.Models.User.Command.SignUp;
using AipsCore.Application.Models.User.Query.GetUser;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result.IdValue);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> LogIn(LogInUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("test")]
    public async Task Test(IMessagePublisher publisher)
    {
        await publisher.PublishAsync("Test poruka");
    }
}