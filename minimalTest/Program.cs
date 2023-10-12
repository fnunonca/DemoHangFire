using minimalTest;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();

app.MapGet("/", () => "Hello World!");
app.MapPost("/testIpn", (TestIpnRequestDto request) =>
{
    string payload = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
    File.AppendAllText("log.txt", payload + Environment.NewLine);
    return Results.Ok("Payload registrado.");
});

app.UseSwaggerUI();
app.Run();
