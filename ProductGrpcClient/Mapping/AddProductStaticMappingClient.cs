using Mapster;
using ProductGrpc.Protos;

namespace ProductGrpcClient.Mapping;

public static class AddProductStaticMappingClient  
{
    public static AddProductRequest MapToAddProductRequest(this ProductModel model)
    {
        TypeAdapterConfig<ProductModel, AddProductRequest>.NewConfig().IgnoreNullValues(true);
        AddProductRequest dto = TypeAdapter.Adapt<ProductModel, AddProductRequest>(model);
        return dto;
    }
}
