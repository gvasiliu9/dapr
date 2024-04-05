var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dapr.API.Product v1"));

app.UseHttpsRedirection();

app.MapGet("/products", () =>
{
    var products = Enumerable.Range(1, 5).Select(index =>
        new Product
        (
            $"Product {index}"
        ))
        .ToArray();
    return products;
})
.WithName("Get products")
.WithOpenApi();

app.Run();

record Product(string Name);
