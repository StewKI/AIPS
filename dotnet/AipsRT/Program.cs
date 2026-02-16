using AipsCore.Infrastructure.DI;
using AipsRT.Hubs;
using AipsRT.Model.Whiteboard;
using DotNetEnv;
using Microsoft.AspNetCore.SignalR;

Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSignalR();

builder.Services.AddAips(builder.Configuration);

builder.Services.AddScoped<GetWhiteboardService>();
builder.Services.AddSingleton<WhiteboardManager>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.MapGet("/test", (IHubContext<TestHub> hubContext) =>
{
    hubContext.Clients.All.SendAsync("ReceiveText", "Ide gas! ");
});

app.UseCors("frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<TestHub>("/testhub");
app.MapHub<WhiteboardHub>("/whiteboardhub");

app.Run();