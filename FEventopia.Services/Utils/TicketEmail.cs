﻿using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using IronQr;
using IronSoftware.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FEventopia.Services.Utils
{
    public class TicketEmail
    {
        public static string EmailContent(TicketModel ticketModel, AccountModel account, string checkinUrl)
        {
            string style = @"<style>
                                body {
                                    margin: 0;
                                    padding: 0;
                                    font-family: Arial, sans-serif;
                                    height: 100%;
                                    display: flex;
                                    flex-direction: column;
                                }

                                .container {
                                    max-width: 960px;
                                    margin: 0 auto;
                                    padding: 20px;
                                }

                                .invoice-header {
                                    background: #ff914d;
                                    display: flex;
                                    width: 100%;
                                    align-items: center;
                                    height: 60px;
                                    padding: 5px 20px;
                                }

                                .invoice-header-logo img {
                                    width: 60%;
                                    object-fit: contain;
                                }

                                .invoice-body {
                                    background: #fff;
                                    width: 100%;
                                    padding: 40px 20px;
                                }

                                .invoice_title {
                                    margin-top: 0;
                                    margin-bottom: 0;
                                    font-size: 24px;
                                    font-weight: 600;
                                    color: #000;
                                    text-align: left;
                                }

                                .vhls140 {
                                    width: 100%;
                                    margin-top: 30px;
                                }

                                .vhls140 ul {
                                    list-style: none;
                                    padding: 0;
                                }

                                .vhls140 li {
                                    margin-bottom: 5px;
                                }

                                .vdt-list {
                                    font-size: 14px;
                                    font-weight: 400;
                                    color: #717171;
                                    text-align: left;
                                    line-height: 24px;
                                }

                                .table-responsive {
                                    overflow-x: auto;
                                }

                                table {
                                    width: 100%;
                                    border-collapse: collapse;
                                }

                                th,
                                td {
                                    padding: 12px;
                                    border: 1px solid #ddd;
                                    text-align: left;
                                }

                                th {
                                    background: #333;
                                    color: #fff;
                                }

                                .totalinv2 {
                                    font-size: 18px;
                                    font-weight: 500;
                                    color: #000;
                                    margin-bottom: 8px;
                                }

                                .text-end {
                                    text-align: right;
                                }

                                .pe-xl-4 {
                                    padding-right: 1.5rem;
                                }

                                .cut-line {
                                    position: relative;
                                    border-bottom: 1px dashed #bbb;
                                    height: 1px;
                                    margin: 30px 0;
                                }

                                .cut-line i {
                                    position: absolute;
                                    top: -6px;
                                    background: #fff;
                                    width: 16px;
                                }

                                .row {
                                    display: flex;
                                    flex-wrap: wrap;
                                    margin-right: -15px;
                                    margin-left: -15px;
                                }

                                .col-lg-7,
                                .col-lg-5,
                                .col-md-6,
                                .col-md-10,
                                .col-lg-8,
                                .col-md-12 {
                                    position: relative;
                                    width: 100%;
                                    padding: 0 15px;
                                }

                                .col-lg-7 {
                                    flex: 0 0 58.33333%;
                                    max-width: 58.33333%;
                                }

                                .col-lg-5 {
                                    flex: 0 0 41.66667%;
                                    max-width: 41.66667%;
                                }

                                .col-md-6 {
                                    flex: 0 0 50%;
                                    max-width: 50%;
                                }

                                .col-md-10 {
                                    flex: 0 0 83.33333%;
                                    max-width: 83.33333%;
                                }

                                .col-lg-8 {
                                    flex: 0 0 66.66667%;
                                    max-width: 66.66667%;
                                }

                                .col-md-12 {
                                    flex: 0 0 100%;
                                    max-width: 100%;
                                }

                                .event-thumbnail-img img,
                                .QR-scanner img {
                                    width: 100%;
                                    object-fit: cover;
                                }

                                .event-order-dt-content {
                                    padding: 20px;
                                }

                                .buyer-name,
                                .booking-total-tickets,
                                .booking-total-grand {
                                    margin-top: 10px;
                                }

                                .booking-total-tickets {
                                    display: flex;
                                    align-items: center;
                                }

                                .booking-total-tickets i {
                                    margin-right: 10px;
                                }

                                .QR-counter-type {
                                    list-style: none;
                                    padding: 0;
                                }

                                .QR-counter-type li {
                                    color: #000;
                                    margin-bottom: 17px;
                                    font-weight: 500;
                                    font-size: 14px;
                                    font-style: italic;
                                }

                                .QR-dt {
                                    background: #f9f9f9;
                                    padding: 20px;
                                    height: 100%;
                                }

                                .main-card {
                                    border: 1px solid #ddd;
                                    border-radius: 8px;
                                    overflow: hidden;
                                    margin-bottom: 30px;
                                }

                                .main-table {
                                    margin-top: 40px;
                                }

                                .row {
                                    margin: 0; /* Remove default margin for better layout control */
                                }

                                /* Style the columns */
                                .col {
                                    display: flex; /* Enable flexbox for responsive sizing */
                                    flex-direction: column; /* Stack content vertically */
                                    padding: 15px; /* Add some padding for spacing */
                                    box-sizing: border-box; /* Ensure padding doesn't affect width calculations */
                                }

                                .g-0 {
                                    display: flex;
                                }

                                .invoice_title {
                                    width: 100%;
                                }

                                .left-side-foot {
                                    width: 60%;
                                    padding: 0px;
                                }

                                .right-side-foot {
                                    width: 40%;
                                    padding: 0px;
                                }
                            </style>";
			string body =
		     $@"<!DOCTYPE html>
                <html lang=""en"" class=""h-100"">
                <head>
                    <meta charset=""utf-8"">
                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                    <meta name=""viewport"" content=""width=device-width"">
                    {style}
                </head>
                <body class=""d-flex flex-column h-100"">
                    <!-- Invoice Start-->
                    <div class=""invoice clearfix"">
                        <div class=""container"">
                            <div class=""justify-content-md-center"">
                                <div class=""col-lg-8 col-md-10"">
                                    <div class=""invoice-header"">
                                        <div class=""invoice-header-logo"">
                                            <img src=""https://firebasestorage.googleapis.com/v0/b/feventopia-app.appspot.com/o/logo%2Flogo.svg?alt=media&token=6e50aaa8-2c91-4596-9b11-e407bb6694e3"">
                                        </div>
                                    </div>
                                    <div class=""invoice-body"">
                                        <div class=""invoice_dts"">
                                            <div class=""row"">
                                                <div class=""col-md-12"">
                                                    <h2 class=""invoice_title"">HÓA ĐƠN THANH TOÁN</h2>
                                                </div>
                                                <div class=""col-md-6"">
                                                    <div class=""vhls140"">
                                                        <ul>
                                                            <li>
                                                                <div class=""vdt-list"">Tên khách hàng : {account.Name}</div>
                                                            </li>
                                                            <li>
                                                                <div class=""vdt-list"">Số điện thoại : {account.PhoneNumber}</div>
                                                            </li>
                                                            <li>
                                                                <div class=""vdt-list"">Địa chỉ email : {account.Email}</div>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                                <div class=""col-md-6"">
                                                    <div class=""vhls140"">
                                                        <ul>
                                                            <li>
                                                                <div class=""vdt-list"">Số hóa đơn : {ticketModel.Transaction.Id}</div>
                                                            </li>
                                                            <li>
                                                                <div class=""vdt-list"">Ngày đặt : {ticketModel.Transaction.TransactionDate.ToString("dd/MM/yyyy")}</div>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class=""main-table"">
                                            <div class=""table-responsive"">
                                                <table class=""table"">
                                                    <thead class=""thead-dark"">
                                                        <tr>
                                                            <th scope=""col"">#</th>
                                                            <th scope=""col"">Thông tin sự kiện</th>
                                                            <th scope=""col"">Loại</th>
                                                            <th scope=""col"">Tổng</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td>1</td>
                                                            <td><a href=""https://feventopia.vercel.app/event/{ticketModel.Event.Id}"" target=""_blank"">{ticketModel.Event.EventName}</a></td>
                                                            <td>{ticketModel.Event.Category}</td>
                                                            <td>{string.Format("{0:n0}", ticketModel.Transaction.Amount)} VNĐ</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan=""1""></td>
                                                            <td colspan=""5"">
                                                                <div class=""user_dt_trans text-end pe-xl-4"">
                                                                    <div class=""totalinv2"">Tổng hóa đơn : {string.Format("{0:n0}", ticketModel.Transaction.Amount)} VNĐ</div>
                                                                    <p>Thanh toán qua FEventWallet</p>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div class=""invoice_footer"">
                                            <div class=""cut-line"">
                                                <i class=""fa-solid fa-scissors""></i>
                                            </div>
                                            <div class=""main-card"">
                                                <div class=""g-0 "">
                                                    <div class=""col-lg-7 left-side-foot"">
                                                        <div class=""event-order-dt"">
                                                            <div class=""event-thumbnail-img"">
                                                                <img src=""{ticketModel.Event.Banner}""
                                                                    alt=""event-banner"">
                                                            </div>
                                                            <div class=""event-order-dt-content"">
                                                                <h5>{ticketModel.Event.EventName}</h5>
                                                                <span>Từ  : {ticketModel.EventDetail.StartDate.ToString("dddd, dd/MM/yyyy, HH:mm:ss", CultureInfo.InvariantCulture)}</span>
                                                                <br>
                                                                <span>Đến : {ticketModel.EventDetail.EndDate.ToString("dddd, dd/MM/yyyy, HH:mm:ss", CultureInfo.InvariantCulture)}</span>
                                                                <div class=""buyer-name"">{account.Name}</div>
                                                                <div class=""booking-total-grand"">
                                                                    Tổng : <span>{string.Format("{0:n0}", ticketModel.Transaction.Amount)} VNĐ</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class=""col-lg-5 right-side-foot"">
                                                        <div class=""QR-dt"">
                                                            <ul class=""QR-counter-type"">
                                                                <li>Mã QR code được sử dụng để check in tại sự kiện, vui lòng không chia
                                                                    sẻ mã này cho người khác</li>
                                                                <li>Ghế ngồi ngẫu nhiên.</li>
                                                                <li>Mã QR dành cho sự kiện {ticketModel.Event.EventName} của khách hàng {account.Name}</li>
                                                            </ul>
                                                            <div class=""QR-scanner"">
                                                                <img src=""https://api.qrserver.com/v1/create-qr-code/?size=250x250&data={checkinUrl}""
                                                                    alt=""QR-Ticket"">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class=""cut-line"">
                                                <i class=""fa-solid fa-scissors""></i>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Invoice End-->
                </body>
                </html>";
            return body;
        }
    }
}
