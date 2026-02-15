using Microsoft.AspNetCore.SignalR;

namespace AipsRT.Hubs;

public class TestHub : Hub
{
    public async Task SendText(string text)
    {
        await Clients.All.SendAsync("ReceiveText", text);
    }
}