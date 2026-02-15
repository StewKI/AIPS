using AipsCore.Application.Abstract;
using AipsCore.Application.Common.Authentication.Dtos;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Authentication;
using AipsCore.Application.Common.Message.TestMessage;
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
    public async Task<ActionResult<LogInUserResultDto>> LogIn(LogInUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Execute(command, cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh-login")]
    public async Task<ActionResult<LogInUserResultDto>> RefreshLogIn(RefreshLogInCommand command, CancellationToken cancellationToken)
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

    [AllowAnonymous]
    [HttpPost("test")]
    public async Task Test(IMessagePublisher publisher)
    {
        var test = new TestMessage("ovo je test poruka");
        await publisher.PublishAsync(test);
    }
}