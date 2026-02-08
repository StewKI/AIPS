using AipsCore.Application.Abstract;
using AipsCore.Application.Models.User.Command.CreateUser;
using AipsCore.Domain.Common.Validation;
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
        try
        {
            var userId = await dispatcher.Execute(command, cancellationToken);
            return Ok(userId.IdValue);
        }
        catch (ValidationException validationException)
        {
            return BadRequest(validationException.ValidationErrors);
        }
    }
}