var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// GET /games        vvvvvvvvvvvvvvvvvvvvv handler
app.MapGet("/games", () => "Hello World!");

app.Run();
