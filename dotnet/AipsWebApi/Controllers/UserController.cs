using AipsCore.Application.Abstract;
using AipsCore.Application.Abstract.MessageBroking;
using AipsCore.Application.Common.Message.TestMessage;
using AipsCore.Application.Models.User.Query.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public UserController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [AllowAnonymous]
    [HttpPost("test")]
    public async Task Test(IMessagePublisher publisher)
    {
        var test = new TestMessage("ovo je test poruka");
        await publisher.PublishAsync(test);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetUserQueryResult>> GetUser(string userId, CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(userId);
        var result = await _dispatcher.Execute(query, cancellationToken);

        return Ok(result);
    }
}