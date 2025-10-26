using kebabCards.Models;
using kebabCards.Models.Dtos;
using kebabCards.Services;
using kebabCards.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace kebabCards.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private ApiResponse _response;
        public ProductController(IProductService productService)
        {
            _productService = productService;
            _response = new ApiResponse();
        }

        [Authorize]
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<ApiResponse>> GetAllProducts()
        {
            try
            {
                _response.Result = _productService.GetAllProducts();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.ReturnMessage = "Poprawnie wczytano liste produktów";
            }
            catch (Exception ex)
            {
                _response.Errors.Add(ex.ToString());
                _response.ReturnMessage = "Coś poszlo nie tak";
                _response.IsSuccess = false;
            }
            
            return Ok(_response);
        }

        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpPost("AddProduct")]
        public async Task<ActionResult<ApiResponse>> AddProduct([FromBody]ProductDto productDto)
        {
            try
            {
                _response.ReturnMessage = _productService.AddProduct(productDto);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = productDto;
                _response.IsSuccess = true;
                _response.ReturnMessage = "Produkt został poprawnie dodany.";
            }
            catch (Exception ex)
            {
                _response.Errors.Add(ex.ToString());
                _response.ReturnMessage = "Coś poszlo nie tak";
                _response.IsSuccess = false;
            }
            return Ok(_response);
        }

        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ApiResponse>> UpdateProduct(int productId, [FromBody]ProductDto productDto)
        {
            try
            {
                _response.ReturnMessage = _productService.UpdateProduct(productId, productDto);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = productDto;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Errors.Add(ex.ToString());
                _response.ReturnMessage = "Coś poszlo nie tak";
                _response.IsSuccess = false;
            }
            return Ok(_response);
        }

        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpPut("DeleteProduct")]
        public async Task<ActionResult<ApiResponse>> DeleteProduct(int productId)
        {
            try
            {
                _response.ReturnMessage = _productService.DeleteProduct(productId);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Result = productId;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Errors.Add(ex.ToString());
                _response.ReturnMessage = "Coś poszlo nie tak";
                _response.IsSuccess = false;
            }
            return Ok(_response);
        }
    }
}
