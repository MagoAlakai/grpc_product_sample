using ProductGrpc.API.Services;

var builder = WebApplication.CreateBuilder(args);
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(opt =>
    opt.EnableDetailedErrors = true);

builder.Services.AddDbContext<ProductsContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ProductsContext))));

var app = builder.Build();

SeedDatabase(app);

static void SeedDatabase(IHost host)
{
    using IServiceScope scope = host.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;
    ProductsContext productsContext = services.GetRequiredService<ProductsContext>();
    ProductsContextSeed.SeedAsync(productsContext);
}

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
