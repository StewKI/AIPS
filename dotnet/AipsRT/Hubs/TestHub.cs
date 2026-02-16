using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AipsRT.Hubs;

[Authorize]
public class TestHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"LOOOOOOOOOG: [{Context.UserIdentifier}] User identifier connected");
        Console.WriteLine($"LOOOOOG222: [{Context.User?.Identity?.Name}] User identity name connected");
        
        await base.OnConnectedAsync();
    }

    public async Task SendText(string text)
    {
        await Clients.All.SendAsync("ReceiveText", text);
    }
}