namespace ProductGrpc.Database
{
    public class ProductsContextSeed
    {
        public static void SeedAsync(ProductsContext productsContext)
        {
            if (!productsContext.Product.Any())
            {
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
                productsContext.Product.AddRange(products);
                productsContext.SaveChanges();
            }
        }
    }
}
