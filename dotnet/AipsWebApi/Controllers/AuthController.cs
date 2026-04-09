using AipsCore.Application.Abstract;
using AipsCore.Application.Models.User.Command.LogIn;
using AipsCore.Application.Models.User.Command.LogOut;
using AipsCore.Application.Models.User.Command.LogOutAll;
using AipsCore.Application.Models.User.Command.RefreshLogIn;
using AipsCore.Application.Models.User.Command.SignUp;
using AipsCore.Application.Models.User.Query.GetMe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public AuthController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<ActionResult<SignUpUserCommandResult>> SignUp(SignUpUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LogInUserCommandResult>> LogIn(LogInUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh-login")]
    public async Task<ActionResult<LogInUserCommandResult>> RefreshLogIn(RefreshLogInCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> LogOut(LogOutCommand command, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(command, cancellationToken);
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("logout-all")]
    public async Task<IActionResult> LogOutAll(LogOutAllCommand command, CancellationToken cancellationToken)
    {
        await _dispatcher.Execute(command, cancellationToken);
        return Ok();
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<GetMeQueryResult>> GetMe(CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(new GetMeQuery(), cancellationToken);
        return result;
    }
}