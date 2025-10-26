using kebabCards.Data;
using kebabCards.Models;
using kebabCards.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kebabCards.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context) {
            _context = context;
        }

        public string AddProduct([FromBody]ProductDto productDto)
        {
            Product product = new Product
            {
                Name = productDto.Name,
                PointsEarned = productDto.PointsEarned,
                PointsCost = productDto.PointsCost,
                IsActive = true
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return "Produkt dodany.";
        }

        public string DeleteProduct(int productId)
        {
            Product? product = _context.Products.Find(productId);
            if (product == null)
            {
                return "Produkt nie został znaleziony";
            }
            product.IsActive = false;
            _context.Products.Update(product);
            _context.SaveChanges();
            return "Produkt został usunięty";
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.Where(p => p.IsActive).ToList();
        }

        public string UpdateProduct(int productId, ProductDto productDto)
        {
            Product? product = _context.Products.Find(productId);
            if (product == null)
            {
                return "Produkt nie został znaleziony.";
            }
            product.PointsCost = productDto.PointsCost;
            product.Name = productDto.Name;
            product.PointsEarned = productDto.PointsEarned;
            _context.Products.Update(product);
            _context.SaveChanges();
            return "Produkt został poprawnie zmodyfikowany.";
        }
    }
}
