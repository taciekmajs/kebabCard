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
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionSerivce _transatcionService;
        private ApiResponse _response;
        public TransactionController(ITransactionSerivce transatcionService)
        {
            _transatcionService = transatcionService;
            _response = new ApiResponse();
        }
        [Authorize]
        [HttpPost("PerformTransaction")]
        public async Task<ActionResult<ApiResponse>> PerformTransaction(PerformTransactionDto transactionDto)
        {
            try
            {
                _response.ReturnMessage = _transatcionService.PerformTransaction(transactionDto);
                _response.Result = transactionDto;
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
        [Authorize]
        [HttpDelete("CancelTransaction")]
        public async Task<ActionResult<ApiResponse>> CancelTransaction(int transactionId)
        {
            try
            {
                _response.ReturnMessage = _transatcionService.CancelTransaction(transactionId);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.Result = transactionId;
                _response.IsSuccess = true;
                if (_response.ReturnMessage != "Anulowanie transakcji powiodło się.") _response.IsSuccess = false;

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
        [HttpGet("GetTodayTransactionsForUser")]
        public async Task<ActionResult<ApiResponse>> GetTodayTransactionsForUser(string userId)
        {
            try
            {
                _response.Result = _transatcionService.GetTodayTransactionsForUser(userId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.ReturnMessage = "Poprawnie wczytano listę transakcji";
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
        [HttpGet("GetTransatcionsByDate")]
        public async Task<ActionResult<ApiResponse>> GetTransatcionsByDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                _response.Result = _transatcionService.GetTransatcionsByDate(startDate, endDate);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.ReturnMessage = "Poprawnie wczytano listę transakcji";
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
        [HttpGet("GetTransationItemSeparatedByProduct")]
        public async Task<ActionResult<ApiResponse>> GetTransationItemSeparatedByProduct(DateTime startDate, DateTime endDate)
        {
            try
            {
                _response.Result = _transatcionService.GetTransationItemSeparatedByProduct(startDate, endDate);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.ReturnMessage = "Poprawnie wczytano listę transakcji";
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
