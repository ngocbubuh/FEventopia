using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FEventopia.Controllers.Controllers
{
    [Route("payment")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly ITransactionService _transactionService;

        public TransactionController(IVnPayService vnPayService, ITransactionService transactionService)
        {
            _vnPayService = vnPayService;
            _transactionService = transactionService;
        }
        private string GetCurrentLogin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return AuthenToolSetting.GetCurrentUsername(identity);
        }

        [HttpPost("Recharge")]
        [Authorize]
        public async Task<IActionResult> Recharge(double amount)
        {
            try
            {
                var username = GetCurrentLogin();
                var transaction = await _transactionService.AddTransactionByVNPAYAsync(amount, username);
                var paymentURL = _vnPayService.CreatePaymentUrl(amount, transaction, HttpContext);
                return Ok(paymentURL);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("VNPayReturn")]
        public async Task<IActionResult> VnPayReturn([FromQuery] VnPayModel model)
        {
            try
            {
                var transaction = await _transactionService.UpdateTransactionByVNPAYStatusAsync(model);
                if (transaction != null)
                {
                    var urlParameter = transaction.ToUrlParameters();
                    return Ok(urlParameter);
                } else
                {
                    return BadRequest();
                }
                
            } catch 
            {
                return BadRequest();    
            }
        }
    }
}
