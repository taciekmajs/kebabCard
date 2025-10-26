using kebabCards.Models;
using kebabCards.Models.Dtos;

namespace kebabCards.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        string AddProduct(ProductDto productDto);
        string UpdateProduct(int productId, ProductDto productDto);
        string DeleteProduct(int productId);
    }
}
