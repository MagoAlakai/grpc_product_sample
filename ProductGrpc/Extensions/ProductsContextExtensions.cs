﻿namespace ProductGrpc.Extensions;

public static class ProductsContextExtensions
{
    /// <summary>
    /// If no data is present in the products contest, this will seed the data
    /// </summary>
    /// <param name="productsContext"></param>
    public static void SeedAsync(this ProductsContext ctx)
    {
        bool can_connect = ctx.Database.CanConnect();
        if (can_connect is false)
        {
            ctx.Database.EnsureCreated();
        }

        ctx.Database.Migrate();
        if (ctx.Product.Any() is true)
        {
            return;
        }

        List<Product> products = new()
        {
            new Product
            {
                ProductId = 1,
                Name = "Mi10T",
                Description = "New Xiamoi Phone Mi10T",
                Price = 699,
                Status = Models.ProductStatus.INSTOCK,
                CreatedTime = DateTime.Now
            },
            new Product
            {
                ProductId = 2,
                Name = "P40",
                Description = "New Huawei Phone P40",
                Price = 899,
                Status = Models.ProductStatus.INSTOCK,
                CreatedTime = DateTime.Now
            },
            new Product
            {
                ProductId = 3,
                Name = "A50",
                Description = "New Samsung Phone A50",
                Price = 399,
                Status = Models.ProductStatus.INSTOCK,
                CreatedTime = DateTime.Now
            }
        };
        ctx.Product.AddRange(products);
        ctx.SaveChanges();
    }
}
