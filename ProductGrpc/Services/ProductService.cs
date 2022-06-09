using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductGrpc.Mapping;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ProductsContext _productsContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductsContext? productsContext, ILogger<ProductService>? logger)
        {
            _productsContext = productsContext ?? throw new ArgumentNullException(nameof(productsContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Task<Empty> Test(Empty request, ServerCallContext context)
        {
            return base.Test(request, context);
        }

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            Product? product = await _productsContext.Product.FindAsync(request.ProductId);
            if (product is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID={request.ProductId} is not found"));
            }
                
            ProductModel product_model = product.MapToProductModel();

            GetProductResponse get_product_response = product_model is null
                ? new GetProductResponse
                {
                    Success = false,
                    ProductModel = product_model,
                }
                : new GetProductResponse
                {
                    Success = true,
                    ProductModel = product_model,
                };

            return get_product_response;
        }

        public override async Task<ProductModelListResponse> GetAllProducts(GetAllProductsRequest request, ServerCallContext context)
        {
            List<Product>? product_list = await _productsContext.Product.ToListAsync();

            IEnumerable<ProductModel>? product_model_list = product_list.Select(x => x.MapToProductModel());


            ProductModelListResponse get_all_products_response = product_model_list is null
                ? new ProductModelListResponse
                {
                    Success = false,
                    ProductModelList = { product_model_list },
                }
                : new ProductModelListResponse
                {
                    Success = true,
                    ProductModelList = { product_model_list },
                };

            return get_all_products_response;
        }

        public override async Task<AddProductResponse> AddProduct(AddProductRequest request, ServerCallContext context)
        {

            Product product = request.MapToAddProductModelFromRequest();

            _productsContext.Add(product);
            await _productsContext.SaveChangesAsync();

            ProductModel product_model = product.MapToProductModel();

            AddProductResponse add_product_response = product_model is null
                ? new AddProductResponse
                {
                    Success = false,
                    ProductModel = product_model,
                }
                : new AddProductResponse
                {
                    Success = true,
                    ProductModel = product_model,
                };

            return add_product_response;
        }

        public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            Product product = request.MapToUpdateProductModelFromRequest();

            bool product_exist = await _productsContext.Product.AnyAsync(p => p.ProductId.Equals(product.ProductId));
            if (product_exist is false)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID={product.ProductId} is not found"));
            }

            _productsContext.Update(product);
            await _productsContext.SaveChangesAsync();

            ProductModel product_model = product.MapToProductModel();

            UpdateProductResponse update_product_response = product_model is null
                ? new UpdateProductResponse
                {
                    Success = false,
                    ProductModel = product_model,
                }
                : new UpdateProductResponse
                {
                    Success = true,
                    ProductModel = product_model,
                };

            return update_product_response;
        }

        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            Product? product = await _productsContext.Product.FindAsync(request.ProductId);

            if (product is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID={request.ProductId} is not found"));
            }

            _productsContext.Product.Remove(product);
            int deleteCount = await _productsContext.SaveChangesAsync();

            DeleteProductResponse update_product_response = new DeleteProductResponse
            {
                Success = deleteCount > 0
            };

            return update_product_response;
        }
    }
}
