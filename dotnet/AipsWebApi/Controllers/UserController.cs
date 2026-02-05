using AipsCore.Application.Abstract;
using AipsCore.Application.Models.User.Command.CreateUser;
using AipsCore.Domain.Models.User.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> CreateUser(CreateUserCommand command, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var userId = await dispatcher.Execute<UserId>(command, cancellationToken);

        return Ok(userId.IdValue);
    }
}