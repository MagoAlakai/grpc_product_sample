
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(opt =>
    opt.EnableDetailedErrors = true);

builder.Services.AddDbContext<ProductsContext>(options => {
    string? connection_str = builder.Configuration.GetConnectionString(nameof(ProductsContext));
    ArgumentNullException.ThrowIfNull(connection_str);
    options.UseNpgsql(connection_str);
});

WebApplication app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;
ProductsContext products_context = services.GetRequiredService<ProductsContext>();
//products_context?.Database.Migrate();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//app.SeedDatabase();
app.Run();
