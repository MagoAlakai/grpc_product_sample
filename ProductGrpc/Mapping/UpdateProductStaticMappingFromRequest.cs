using ProductGrpc.Protos;

namespace ProductGrpc.Mapping;

public static class UpdateProductStaticMappingFromRequest
{
    public static Product MapToUpdateProductModelFromRequest(this UpdateProductRequest update_product_request)
    {
        TypeAdapterConfig<UpdateProductRequest, Product>.NewConfig().IgnoreNullValues(true)
            .Map(x => x.Name, src => src.Product.Name)
            .Map(x => x.Description, src => src.Product.Description)
            .Map(x => x.ProductId, src => src.Product.ProductId)
            .Map(x => x.CreatedTime, src => src.Product.CreatedTime.ToDateTime().ToUniversalTime())
            .Map(x => x.Price, src => src.Product.Price);
        DefaultStaticMappingRules.SetDefaultMappings();
        Product product = TypeAdapter.Adapt<UpdateProductRequest, Product>(update_product_request);
        return product;
    }
}
