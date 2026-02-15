using AipsRT.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

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
app.MapHub<TestHub>("/testhub");

app.Run();