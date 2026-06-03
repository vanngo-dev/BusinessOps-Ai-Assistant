using BusinessOps.Api.Data;
using Microsoft.EntityFrameworkCore;
using BusinessOps.Api.Services;

var builder = WebApplication.CreateBuilder(args);
const string FrontendCorsPolicy = "FrontendCorsPolicy";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<OperationsInsightsService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                {
                    return false;
                }

                var isLocalViteHost = uri.Host == "localhost" || uri.Host == "127.0.0.1";
                var isViteDevPort = uri.Port >= 5173 && uri.Port <= 5199;

                return isLocalViteHost && isViteDevPort;
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(FrontendCorsPolicy);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "ok",
        app = "BusinessOps AI Assistant API"
    });
})
.WithName("HealthCheck");
app.MapControllers();
app.Run();
