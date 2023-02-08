namespace ProductGrpc.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Creates the database context and forwards onto the method <see cref="ProductsContextExtensions.SeedAsync(ProductsContext)"/> 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication SeedDatabase(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        ProductsContext products_context = services.GetRequiredService<ProductsContext>();
        //products_context.SeedAsync();
        return app;
    }
}
