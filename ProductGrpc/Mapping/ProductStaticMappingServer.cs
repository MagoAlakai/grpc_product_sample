using ProductGrpc.Protos;

namespace ProductGrpc.Mapping;

public static class ProductStaticMappingServer
{
    public static ProductModel MapToProductModel(this Product product)
    {
        TypeAdapterConfig<Product, ProductModel>.NewConfig().IgnoreNullValues(true);
        DefaultStaticMappingRules.SetDefaultMappings();
        ProductModel product_model = TypeAdapter.Adapt<Product, ProductModel>(product);
        return product_model;
    }
}
