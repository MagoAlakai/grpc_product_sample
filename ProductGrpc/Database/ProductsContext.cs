namespace ProductGrpc.Database;

public class ProductsContext : DbContext
{
    public DbSet<Product> Product { get; set; }

    public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        => Product = Set<Product>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder model_builder)
    {
        base.OnModelCreating(model_builder);

    }
}
