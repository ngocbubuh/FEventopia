using FEventopia.Services.BussinessModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IPaymentService
    {
        string CreatePaymentUrl(TransactionModel model, HttpContext context);
    }
}
