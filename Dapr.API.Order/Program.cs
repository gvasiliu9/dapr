using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dapr.API.Order v1"));


// Get products
app.MapGet("/order", async () =>
{
    var daprClient = app.Services.GetRequiredService<DaprClient>();
    var products = await daprClient.InvokeMethodAsync<Product[]>(HttpMethod.Get, "product", "products");

    return products;
})
.WithName("Get products")
.WithOpenApi();

// Save state
app.MapPost("/save-state", async (string key, string value) =>
{
    var daprClient = app.Services.GetRequiredService<DaprClient>();
    await daprClient.SaveStateAsync("statestore", key, value);
})
.WithName("Save state")
.WithOpenApi();

// Get state
app.MapGet("/get-state", async (string key) =>
{
    var daprClient = app.Services.GetRequiredService<DaprClient>();
    var value = await daprClient.GetStateAsync<string>("statestore", key);

    return value;
})
.WithName("Get state")
.WithOpenApi();

// Publish event
app.MapPost("/publish-event", async (string message) =>
{
    using var client = new DaprClientBuilder().Build();
    await client.PublishEventAsync("redis-pubsub", "orders", new { Message = message });
})
.WithName("Publish event")
.WithOpenApi();

// Subscribe to event
app.MapSubscribeHandler();

app.MapPost("/orders", (Event message) =>
{
    var json = System.Text.Json.JsonSerializer.Serialize(message);
    Console.WriteLine($"Received event: {message.Data}");
})
.WithTopic("redis-pubsub", "orders")
.WithName("orders")
.WithOpenApi();

await app.RunAsync();

record Product(DateOnly Date);

public record Event(string Id,
                    string Source,
                    string Specversion,
                    string Type,
                    string Datacontenttype,
                    object Data,
                    string Subject,
                    string Topic,
                    string Pubsubname);
