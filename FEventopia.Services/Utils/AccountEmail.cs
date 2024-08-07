﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Utils
{
    public class AccountEmail
    {
        public static string EmailContent(string name, string username, string password)
        {
            string body =
                $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""utf-8"">
                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <meta name=""description"" content=""FEventopia"">
                    <meta name=""author"" content=""FEventopia"">
                    <title>Xác nhận Email FEventopia</title>
                </head>
                <body style=""font-family: 'Roboto', sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;"">
                    <div style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 50px #ff914d;"">
                        <div style=""text-align: center; border-bottom: 1px solid #dddddd; background-color: #ff914d; padding-bottom: 20px;"">
                            <img src=""https://firebasestorage.googleapis.com/v0/b/feventopia-app.appspot.com/o/logo%2Flogo-fav.png?alt=media&token=9d2d758d-348b-4965-a506-367c34e2e051"" alt=""Logo"" style=""max-width: 150px;"">
                        </div>
                        <div style=""padding: 20px; text-align: center;"">
                            <h2 style=""color: #333333;"">XÁC NHẬN EMAIL</h2>
                            <p style=""color: #666666; line-height: 1.6;"">Xin chào <strong>{name}</strong>,</p>
                            <p style=""color: #666666; line-height: 1.6;"">Chào mừng bạn đến với FEventopia.</p>
                            <p style=""color: #666666; line-height: 1.6;"">Tài khoản của bạn được hiển thị dưới đây:</p>
                            <p style=""display: inline-block; margin-top: 20px; margin-bottom: 30px; padding: 10px 20px; background-color: #ff0000; color: #ffffff; text-decoration: none; border-radius: 4px;"">{username}</p>
                            <p style=""display: inline-block; margin-top: 20px; margin-bottom: 30px; padding: 10px 20px; background-color: #ff0000; color: #ffffff; text-decoration: none; border-radius: 4px;"">{password}</p>
                            <p style=""color: #666666; line-height: 1.6;"">Bạn vui lòng không chia sẻ tài khoản này cho bất kỳ ai.</p>
                            <p style=""color: #666666; line-height: 1.6;"">Nếu không phải bạn, vui lòng phản hồi lại email này để báo cáo sự nhầm lẫn.</p>
                        </div>
                        <div style=""text-align: center; color: #999999; padding-top: 20px; border-top: 1px solid #dddddd;"">
                            <p>© 2024 <strong>FEventopia. All rights reserved. Powered by FPT University.</strong></p>
                        </div>
                    </div>
                </body>
                </html>";
            return body;
        }
    }
}
