using kebabCards.Models;
using kebabCards.Services;
using kebabCards.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace kebabCards.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private ApiResponse _response;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
            _response = new ApiResponse();
        }

        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpGet("GetAllCustomers")] 
        public async Task<ActionResult<ApiResponse>> GetAllCustomers()
        {
            try
            {
                _response.Result = _customerService.GetAllCustomers();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.ReturnMessage = "Poprawnie wczytano liste kart lojalnościowych";
            }
            catch (Exception ex)
            {
                _response.Errors.Add(ex.ToString());
                _response.ReturnMessage = "Coś poszlo nie tak";
                _response.IsSuccess = false;
            }

            return Ok(_response);
        }
        [Authorize]
        [HttpGet("GetCustomer")]
        public async Task<ActionResult<ApiResponse>> GetCustomer(int cardId)
        {
            try
            {
                _response.Result = _customerService.GetCustomer(cardId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.ReturnMessage = $"Poprawnie wczytano kartę lojalnościową numer {cardId}";
            }
            catch (Exception ex)
            {
                _response.Errors.Add(ex.ToString());
                _response.ReturnMessage = "Nie znaleziono karty";
                _response.IsSuccess = false;
            }

            return Ok(_response);
        }

        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpPost("AddCustomers")]
        public async Task<ActionResult<ApiResponse>> AddCustomers(List<int> cardIds)
        {
            try
            {
                _response.ReturnMessage = _customerService.AddCustomers(cardIds);
                _response.Result = cardIds;
                _response.StatusCode = HttpStatusCode.OK;
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
        [HttpDelete("RemoveCustomers")]
        public async Task<ActionResult<ApiResponse>> DeleteCustomers(List<int> cardIds)
        {
            try
            {
                _response.ReturnMessage = _customerService.DeleteCustomers(cardIds);
                _response.Result = cardIds;
                _response.StatusCode = HttpStatusCode.OK;
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
