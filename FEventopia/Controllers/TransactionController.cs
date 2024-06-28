using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FEventopia.Controllers.Controllers
{
    [Route("payment")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IPaymentService _vnPayService;
        private readonly ITransactionService _transactionService;
        private readonly IAuthenService _authenService;

        public TransactionController(IPaymentService vnPayService, ITransactionService transactionService, IAuthenService authenService)
        {
            _vnPayService = vnPayService;
            _transactionService = transactionService;
            _authenService = authenService;
        }

        [HttpPost("Recharge")]
        [Authorize]
        public async Task<IActionResult> Recharge(double amount)
        {
            try
            {
                if (!(amount >= 10000)) { return BadRequest(); }
                var username = _authenService.GetCurrentLogin;
                var transaction = await _transactionService.AddTransactionByVNPAYAsync(amount, username);
                var paymentURL = _vnPayService.CreatePaymentUrl(transaction, HttpContext);
                return Ok(paymentURL);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("VNPayReturn")]
        public async Task<IActionResult> VnPayReturn([FromQuery] VnPayModel model)
        {
            try
            {
                var uri = HttpContext.Request.Host.ToString();
                var transaction = await _transactionService.UpdateTransactionByVNPAYStatusAsync(model);
                var urlParameters = transaction.ToUrlParameters();
                return Redirect("https://feventopia.vercel.app/transactioninfo?" + urlParameters);
                //return Ok(urlParameter);
            } catch 
            {
                throw;    
            }
        }

        [HttpGet("GetAllTransaction")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllTransactionAsync([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _transactionService.GetAllTransactionsAsync(pageParaModel);
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetAllProfileTransaction")]
        [Authorize]
        public async Task<IActionResult> GetAllProfileTransaction([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _transactionService.GetAllTransactionByUsernameAsync(username, pageParaModel);
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetAllUserTransaction")]
        [Authorize]
        public async Task<IActionResult> GetAllUserTransactionAsync([FromQuery] PageParaModel model, string username)
        {
            try
            {
                var result = await _transactionService.GetAllTransactionByUsernameAsync(username, model);
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            } catch
            {
                throw;
            }
        }
    }
}
