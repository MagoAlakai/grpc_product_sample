Console.WriteLine("Server starting...");
Thread.Sleep(2000);

HttpClientHandler http_client_handler = new()
{
    ClientCertificateOptions = ClientCertificateOption.Manual,
    SslProtocols = SslProtocols.Tls12
};
http_client_handler.UseUnsignedServerCertificateValidation();

HttpClient http_client = new(http_client_handler);
GrpcChannel channel = GrpcChannel.ForAddress("https://productgrpc:53443", new()
{
    HttpClient = http_client
});

ProductProtoService.ProductProtoServiceClient? client = new(channel);

//AddProductAsync
Console.WriteLine("AddProductAsync started...");
Thread.Sleep(2000);

ProductModel product_model = new()
{
    ProductId = 4,
    Name = "Oppo X10",
    Description = "New Xiamoi Phone Mi10T",
    Price = 699,
    Status = ProductStatus.Instock,
    CreatedTime = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime())
};

AddProductRequest add_product_request = new()
{
    Product = product_model,
};

AddProductResponse add_product_response = await client.AddProductAsync(add_product_request);
Console.WriteLine("AddProductAsync Response Success: " + add_product_response.Success.ToString());
Console.WriteLine("AddProductAsync Response Model: " + add_product_response.ProductModel.ToString());

//GetProductAsync
Console.WriteLine("GetProductAsync started...");

GetProductRequest product_request = new()
{
    ProductId = 4,
};

GetProductResponse get_product_response = await client.GetProductAsync(product_request);
Console.WriteLine("GetProductAsync Response Success: " + get_product_response.Success.ToString());
Console.WriteLine("GetProductAsync Response Model: " + get_product_response.ProductModel.ToString());


//GetAllProductsAsync
Console.WriteLine("GetAllProductsAsync started...");
Thread.Sleep(2000);

GetAllProductsRequest get_all_product_request = new GetAllProductsRequest();
ProductModelListResponse get_all_products_response = client.GetAllProducts(get_all_product_request);
Console.WriteLine("GetAllProductsAsync Response Success: " + get_all_products_response.Success.ToString());
foreach (var item in get_all_products_response.ProductModelList)
{
    Console.WriteLine(item.ToString());
}

//UpdateProductAsync
Console.WriteLine("UpdateProductAsync started...");
Thread.Sleep(2000);

ProductModel update_product_model = new()
{
    ProductId = 4,
    Name = "Oppo X10 Updated",
    Description = "New Xiamoi Phone Mi10T",
    Price = 699,
    Status = ProductStatus.Instock,
    CreatedTime = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime())
};

UpdateProductRequest update_product_request = new()
{
    Product = update_product_model,
};

UpdateProductResponse update_product_response = await client.UpdateProductAsync(update_product_request);
Console.WriteLine("UpdateProductAsync Response Success: " + update_product_response.Success.ToString());
Console.WriteLine("UpdateProductAsync Response Model: " + update_product_response.ProductModel.ToString());

//DeleteProductAsync
Console.WriteLine("DeleteProductAsync started...");
Thread.Sleep(2000);

DeleteProductRequest delete_product_request = new()
{
    ProductId = 4,
};

DeleteProductResponse delete_product_response = await client.DeleteProductAsync(delete_product_request);
Console.WriteLine("DeleteProductAsync Response Success: " + delete_product_response.Success.ToString() + ", the Product Oppo X10 Updated has been Deleted");

Console.ReadLine();
