using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using FEventopia.Services.Utils;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly VnPaySetting _vnPaySetting;

        public PaymentService(IOptions<VnPaySetting> vnPaySetting)
        {
            this._vnPaySetting = vnPaySetting.Value;
        }
        public string CreatePaymentUrl(TransactionModel transaction, HttpContext context)
        {
            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _vnPaySetting.Version);
            pay.AddRequestData("vnp_Command", _vnPaySetting.Command);
            pay.AddRequestData("vnp_TmnCode", _vnPaySetting.TmnCode);
            pay.AddRequestData("vnp_Amount", ((int) Math.Round(transaction.Amount, 2) * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _vnPaySetting.CurrCode);
            pay.AddRequestData("vnp_IpAddr", Utils.Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _vnPaySetting.Locale);
            pay.AddRequestData("vnp_OrderInfo", transaction.Description);
            pay.AddRequestData("vnp_OrderType", "250000");
            pay.AddRequestData("vnp_ReturnUrl", _vnPaySetting.ReturnUrl);
            pay.AddRequestData("vnp_TxnRef", transaction.Id.ToString());

            var paymentUrl = pay.CreateRequestUrl(_vnPaySetting.BaseUrl, _vnPaySetting.HashSecret);

            return paymentUrl;
        }
    }
}
