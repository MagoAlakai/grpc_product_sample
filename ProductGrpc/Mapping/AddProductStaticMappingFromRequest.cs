using ProductGrpc.Protos;

namespace ProductGrpc.Mapping;

public static class AddProductStaticMappingFromRequest
{
    public static Product MapToAddProductModelFromRequest(this AddProductRequest add_product_request)
    {
        TypeAdapterConfig<AddProductRequest, Product>.NewConfig().IgnoreNullValues(true)
            .Map(x => x.Name, src => src.Product.Name)
            .Map(x => x.Description, src => src.Product.Description)
            .Map(x => x.CreatedTime, src => src.Product.CreatedTime.ToDateTime().ToUniversalTime());
        DefaultStaticMappingRules.SetDefaultMappings();
        Product product = TypeAdapter.Adapt<AddProductRequest, Product>(add_product_request);
        return product;
    }
}
