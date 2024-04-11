using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dapr.API.Order v1"));

app.UseHttpsRedirection();

app.MapGet("/order", async () =>
{
    // Call product API
    var daprClient = app.Services.GetRequiredService<DaprClient>();
    var products = await daprClient.InvokeMethodAsync<Product[]>(HttpMethod.Get, "product", "products");

    // Save state
    await daprClient.SaveStateAsync("statestore", "test", 1);
    var value = await daprClient.GetStateAsync<int>("statestore", "test");

    // Publish event
    // ...

    // Subscribe to event
    // ...

    return products;
})
.WithName("Order products")
.WithOpenApi();

app.Run();

record Product(DateOnly Date);