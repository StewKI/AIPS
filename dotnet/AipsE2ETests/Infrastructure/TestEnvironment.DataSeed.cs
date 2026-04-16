using System.Net.Http.Headers;
using System.Net.Http.Json;
using AipsCore.Application.Models.User.Command.LogIn;
using AipsCore.Application.Models.User.Query.GetMe;
using AipsCore.Application.Models.Whiteboard.Command.CreateWhiteboard;
using AipsCore.Domain.Models.Whiteboard.Enums;
using AipsCore.Domain.Models.WhiteboardMembership.Enums;
using AipsCore.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;
using WhiteboardMembership = AipsCore.Domain.Models.WhiteboardMembership.WhiteboardMembership;

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

    public async Task<List<string>> CreateMultipleWhiteboardsForUser(string email, string password, int whiteboardCount)
    {
        var whiteboards = new List<string>();
        
        for (int i = 1; i <= whiteboardCount; i++)
        {
            whiteboards.Add(await CreateWhiteboardForUser(email, password, $"TestWhiteboard{i}", WhiteboardJoinPolicy.FreeToJoin, 10));
        }
        
        return whiteboards;
    }

    public async Task<string> CreateWhiteboardForUser(string email, string password, string title, WhiteboardJoinPolicy joinPolicy, int maxParticipants)
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
        
        var whiteboardResult = await whiteboardResponse.Content.ReadFromJsonAsync<CreateWhiteboardCommandResult>();
        
        using var logoutRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/auth/logout");
        logoutRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.AccessToken);
        logoutRequest.Content = JsonContent.Create(new
        {
            loginResult.RefreshToken
        });
        
        var logoutResponse = await Client.SendAsync(logoutRequest);
        logoutResponse.EnsureSuccessStatusCode();

        return whiteboardResult!.WhiteboardId;
    }
    
    public async Task CreateMultipleWhiteboardMemberships(string email, string password, List<string> whiteboardIds)
    {
        foreach (var whiteboardId in whiteboardIds)
        {
            await CreateWhiteboardMembership(email, password, whiteboardId);
        }
    }

    public async Task CreateWhiteboardMembership(string email, string password, string whiteboardId)
    {
        var loginResponse = await Client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password
        });

        loginResponse.EnsureSuccessStatusCode();

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LogInUserCommandResult>();

        var meRequest = new HttpRequestMessage(HttpMethod.Get, "api/Auth/me");
        meRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginResult!.AccessToken);

        var meResponse = await Client.SendAsync(meRequest);
        meResponse.EnsureSuccessStatusCode();
        
        var meResult = await meResponse.Content.ReadFromJsonAsync<GetMeQueryResult>();
        var userId = meResult!.UserId;
        
        await using var db = CreateDb();

        var membership = WhiteboardMembership.Create(whiteboardId, userId, false, WhiteboardMembershipStatus.Accepted, DateTime.UtcNow);
        
        var membershipEntity = new AipsCore.Infrastructure.Persistence.WhiteboardMembership.WhiteboardMembership()
        {
            Id = new Guid(membership.Id.IdValue),
            WhiteboardId = new Guid(membership.WhiteboardId.IdValue),
            UserId = new Guid(membership.UserId.IdValue),
            EditingEnabled = membership.EditingEnabled.EditingEnabledValue,
            Status = membership.Status,
            LastInteractedAt = membership.LastInteractedAt.LastInteractedAtValue
        };

        await db.WhiteboardMemberships.AddAsync(membershipEntity);
        await db.SaveChangesAsync();
        
        using var logoutRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/auth/logout");
        logoutRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.AccessToken);
        logoutRequest.Content = JsonContent.Create(new
        {
            loginResult.RefreshToken
        });
        
        var logoutResponse = await Client.SendAsync(logoutRequest);
        logoutResponse.EnsureSuccessStatusCode();
    }
    
    private AipsDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AipsDbContext>()
            .UseNpgsql(_infrastructure.PostgresConnectionString)
            .Options;

        return new AipsDbContext(options);
    }
}