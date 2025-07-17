var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<JsonDbService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
