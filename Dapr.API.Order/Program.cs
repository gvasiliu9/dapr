using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;

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
    var daprClient = app.Services.GetRequiredService<DaprClient>();
    var products = await daprClient.InvokeMethodAsync<Product[]>(HttpMethod.Get, "product", "products");

    return products;
})
.WithName("Order products")
.WithOpenApi();

app.Run();

record Product(DateOnly Date);