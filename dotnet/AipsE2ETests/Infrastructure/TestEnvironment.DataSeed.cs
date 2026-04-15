using System.Net.Http.Headers;
using System.Net.Http.Json;
using AipsCore.Application.Models.User.Command.LogIn;
using AipsCore.Domain.Models.Whiteboard.Enums;

namespace AipsE2ETests.Infrastructure;

public sealed partial class TestEnvironment
{
    public async Task CreateUser(string username, string email, string password)
    {
        var response = await Client.PostAsJsonAsync("/api/auth/signup", new
        {
            username,
            email,
            password
        });

        response.EnsureSuccessStatusCode();
    }

    public async Task CreateMultipleWhiteboardsForUser(string email, string password, int whiteboardCount)
    {
        for (int i = 1; i <= whiteboardCount; i++)
        {
            await CreateWhiteboardForUser(email, password, $"TestWhiteboard{i}", WhiteboardJoinPolicy.FreeToJoin, 10);
        }
    }

    public async Task CreateWhiteboardForUser(string email, string password, string title, WhiteboardJoinPolicy joinPolicy, int maxParticipants)
    {
        var loginResponse = await Client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password
        });

        loginResponse.EnsureSuccessStatusCode();

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LogInUserCommandResult>();
        
        using var whiteboardRequest = new HttpRequestMessage(HttpMethod.Post, "/api/whiteboard");
        whiteboardRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginResult!.AccessToken);
        whiteboardRequest.Content = JsonContent.Create(new
        {
            title,
            maxParticipants = 10,
            joinPolicy
        });
        
        var whiteboardResponse = await Client.SendAsync(whiteboardRequest);
        whiteboardResponse.EnsureSuccessStatusCode();
        
        using var logoutRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/auth/logout");
        logoutRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.AccessToken);
        logoutRequest.Content = JsonContent.Create(new
        {
            loginResult.RefreshToken
        });
        
        var logoutResponse = await Client.SendAsync(logoutRequest);
        logoutResponse.EnsureSuccessStatusCode();
    }
}