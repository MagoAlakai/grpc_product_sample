﻿namespace ProductGrpc.Data
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options) 
        {
            
        }
        public DbSet<Product> Product { get; set; } = default!;
    }
}
