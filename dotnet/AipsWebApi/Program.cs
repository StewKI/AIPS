using AipsCore.Infrastructure.DI;
using AipsCore.Infrastructure.Persistence.Db;
using AipsWebApi.Middleware;
using DotNetEnv;

Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddAips(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

await app.Services.InitializeInfrastructureAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("frontend");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();