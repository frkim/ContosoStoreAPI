using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var sampleProducts = new Product[] {
    new(1, "Bread", 123),
    new(2, "Bottle of water", 345),
    new(3, "Apple", 2898),
    new(4, "Chicken", 243),
    new(5, "Potaoes", 1543)
};

var todosApi = app.MapGroup("/products");
todosApi.MapGet("/", () => sampleProducts);
todosApi.MapGet("/{id}", (int id) =>
    sampleProducts.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();

public record Product(int Id, string? Title, int Quantity = 0);

[JsonSerializable(typeof(Product[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
