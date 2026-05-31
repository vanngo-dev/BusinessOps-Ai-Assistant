var builder = WebApplication.CreateBuilder(args);

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Local dev: keep HTTPS redirection disabled for now
// app.UseHttpsRedirection();

app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "ok",
        app = "BusinessOps AI Assistant API"
    });
})
.WithName("HealthCheck");

app.Run();