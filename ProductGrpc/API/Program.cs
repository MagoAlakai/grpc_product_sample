
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// todo (mago||06-01-2023): Create the README file with the whastup info and Identity-svc one

// Add services to the container.
builder.Services.AddGrpc(opt =>
    opt.EnableDetailedErrors = true);

builder.Services.AddDbContext<ProductsContext>(options => {
    string? connection_str = builder.Configuration.GetConnectionString(nameof(ProductsContext));
    ArgumentNullException.ThrowIfNull(connection_str);
    Console.WriteLine(connection_str);
    options.UseSqlServer(connection_str);
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.SeedDatabase();
app.Run();
