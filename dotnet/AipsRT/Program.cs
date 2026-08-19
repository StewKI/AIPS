using AipsCore.Application.Common.Message.ErrorMessage;
using AipsCore.Infrastructure.DI;
using AipsRT.Hubs;
using AipsRT.Model.Memberships;
using AipsRT.Model.Users;
using AipsRT.Model.Whiteboard;
using AipsRT.Services;
using AipsRT.Services.Interfaces;
using DotNetEnv;

if (File.Exists("../../.env"))
{
    Env.Load("../../.env");
}

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSignalR();

builder.Services.AddAips(builder.Configuration);
builder.Services.AddAipsMessageHandlers();

builder.Services.AddSingleton<IErrorMessageHandleStrategy, RtErrorHandleStrategy>();
builder.Services.AddHostedService<ErrorSubscriberBackgroundService>();

builder.Services.AddTransient<MembershipService>();
builder.Services.AddTransient<UserService>();

builder.Services.AddScoped<GetWhiteboardService>();
builder.Services.AddSingleton<WhiteboardManager>();
builder.Services.AddSingleton<IMessagingService, MessagingService>();

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

app.UseCors("frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<WhiteboardHub>("/hubs/whiteboard");

app.Run();