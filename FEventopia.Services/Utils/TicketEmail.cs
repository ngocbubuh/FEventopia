using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using IronQr;
using IronSoftware.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Utils
{
    public class TicketEmail
    {
        public static string EmailContent(TicketModel ticketModel,AccountModel account, string checkinUrl)
        {
			string body =
				 $@"<!DOCTYPE html>
					<html lang=""en"" class=""h-100"">
						<head>
							<meta charset=""utf-8"">
							<meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
							<meta name=""viewport"" content=""width=device-width, shrink-to-fit=9"">
							<meta name=""description"" content=""FEventopia"">
							<meta name=""author"" content=""FEventopia"">		
							<title>Vé đã mua</title>
		
							<!-- Favicon Icon -->
							<link rel=""icon"" type=""image/png"" href=""images/logo-fav.png"">
		
							<!-- Stylesheets -->
							<link rel=""preconnect"" href=""https://fonts.googleapis.com"">
							<link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
							<link href=""https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&display=swap"" rel=""stylesheet"">
							<link href='vendor/unicons-2.0.1/css/unicons.css' rel='stylesheet'>

							<link href=""css/responsive.css"" rel=""stylesheet"">
							<link href=""css/night-mode.css"" rel=""stylesheet"">
		
							<!-- Vendor Stylesheets -->
							<link href=""vendor/fontawesome-free/css/all.min.css"" rel=""stylesheet"">
							<link href=""vendor/OwlCarousel/assets/owl.carousel.css"" rel=""stylesheet"">
							<link href=""vendor/OwlCarousel/assets/owl.theme.default.min.css"" rel=""stylesheet"">
							<link href=""vendor/bootstrap/css/bootstrap.min.css"" rel=""stylesheet"">
							<link href=""vendor/bootstrap-select/dist/css/bootstrap-select.min.css"" rel=""stylesheet"">		

							<style>
								.invoice-header {{
									background: #ff914d;
									display: flex;
									width: 100%;
									align-items: center;
									height: 60px;
									padding: 5px 20px;
								}}

								.invoice-header-logo img {{
									width: 130px;
								}}

								.download-link {{
									color: rgba(255,255,255,0.9);
									font-size: 16px;
									font-weight: 500;
								}}

								.download-link:hover {{
									color: rgba(255,255,255,1) !important;
								}}

								.invoice-body {{
									padding: 30px;
									background: #fff;
									float: left;
									width: 100%;
								}}

								.invoice_title {{
									margin-top: 0;
									margin-bottom: 0;
									font-size: 24px;
									font-weight: 600;
									color: #000;
									text-align: left;
								}}

								.vhls140 {{
									width: 100%;
									margin-top: 30px;
								}}

								.vhls140 ul li {{
									display: block;
									margin-bottom: 5px;
								}}

								.vdt-list {{
									font-size: 14px;
									font-weight: 400;
									color: #717171;
									text-align: left;
									line-height: 24px;
								}}

								.totalinv2 {{
									font-size: 18px;
									font-weight: 500;
									color: #000;
									margin-bottom: 8px;
								}}

								.user_dt_trans {{
									padding: 10px;
								}}

								.user_dt_trans p {{
									margin-bottom: 0;
								}}

								.cut-line {{
									position: relative;
									border-bottom-width: 1px;
									border-top: 0;
									border-radius: 0;
									border-left: 0;
									border-style: dashed;
									height: 1px;
									color: #bbb;
									margin-top: 30px;
									margin-bottom: 30px;
								}}

								.cut-line i {{
									position: absolute;
									top: -6px;
									background: #fff;
									width: 16px;
								}}

								.QR-dt {{
									height: 100%;
									background: #f9f9f9;
								}}

								.QR-scanner {{
									background: #fff;
									display: inline-block;
									padding: 5px;
									border: 1px solid #efefef;
									border-radius: 3px;
								}}

								.QR-scanner img {{
									width: 235px;
								}}

								.QR-dt p {{
									margin-bottom: 0;
									font-size: 12px;
									margin-top: 5px;
								}}

								.QR-counter-type li {{
									margin-bottom: 3px;
									color: #000;
								}}

								.QR-counter-type li:first-child {{
									margin-bottom: 17px;
									font-weight: 500;
									font-size: 14px;
									font-style: italic;
								}}

								.QR-counter-type li:last-child {{
									margin-bottom: 20px;
									font-weight: 500;
									font-size: 14px;
									font-style: italic;
								}}

								.featured-controls label {{
									margin: 0;
									cursor: pointer;
									text-align: center;
								}}

								.featured-controls label:first-child {{
									margin-left: 0;
								}}

								.featured-controls label span {{
									font-size: 12px;
									color: #717171;
									border: 1px solid #efefef;
									background: #fff;
									height: 32px;
									padding: 7px 20px;
									border-radius: 30px;
									display: inline-block;
									margin-right: 6px;
									margin-bottom: 6px;
								}}

								.featured-controls label input {{
									position:absolute;
									top:-20px;
								}}

								.featured-controls label input {{
									position:absolute;
									top:-20px;
								}}

								.featured-controls input:checked + span {{
									background: #efefef;
									color: #000 !important;
								}}

								.event-box {{
									padding: 0;
									display: none;
									margin-top: 0;
								}}

								.map iframe {{
									width: 100%;
									height: 250px;
								}}
							</style>
						</head>
						<body class=""d-flex flex-column h-100"">
							<!-- Invoice Start-->
							<div class=""invoice clearfix"">
								<div class=""container"">
									<div class=""row justify-content-md-center"">
										<div class=""col-lg-8 col-md-10"">
											<div class=""invoice-header justify-content-between"">
												<div class=""invoice-header-logo"">
													<img src=""images/logo.svg"" alt=""invoice-logo"">
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
																	<li><div class=""vdt-list"">Khách hàng: {account.Name}</div></li>
																	<li><div class=""vdt-list"">Số điện thoại: {account.PhoneNumber}</div></li>
																	<li><div class=""vdt-list"">Email: {account.Email}</div></li>
																</ul>
															</div>
														</div>
														<div class=""col-md-6"">
															<div class=""vhls140"">
																<ul>
																	<li><div class=""vdt-list"">Số Hóa đơn : {ticketModel.Id}</div></li>
																	<li><div class=""vdt-list"">Ngày đặt : {DateTime.Now}</div></li>
																</ul>
															</div>
														</div>
													</div>
												</div>
												<div class=""main-table bt_40"">
													<div class=""table-responsive"">
														<table class=""table"">
															<thead class=""thead-dark"">
																<tr>
																	<th scope=""col"">Thông tin sự kiện</th>
																	<th scope=""col"">Loại</th>
																	<th scope=""col"">Số lượng</th>
																	<th scope=""col"">Tổng</th>
																</tr>
															</thead>
															<tbody>
																<tr>										
																	<td>{ticketModel.Event.EventName}</td>	
																	<td>{ticketModel.Event.Category}</td>	
																	<td>1</td>
																	<td>{ticketModel.Transaction.Amount}</td>
																</tr>
																<tr>
																	<td colspan=""1""></td>
																	<td colspan=""5"">
																		<div class=""user_dt_trans text-end pe-xl-4"">
																			<div class=""totalinv2"">Tổng hóa đơn : {ticketModel.Transaction.Amount}</div>
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
														<div class=""row g-0"">
															<div class=""col-lg-7"">
																<div class=""event-order-dt p-4"">
																	<div class=""event-order-dt-content"">
																		<h5>{ticketModel.Event.EventName}</h5>
																		<span>{ticketModel.EventDetail.StartDate}</span>
																		<div class=""buyer-name"">{account.Name}</div>
																		<div class=""booking-total-grand"">
																			Tổng : <span>{ticketModel.Transaction.Amount}</span>
																		</div>
																	</div>
																</div>
															</div>
															<div class=""col-lg-5"">
																<div class=""QR-dt p-4"">
																	<ul class=""QR-counter-type"">
																		<li>Mã QR code được sử dụng để check in tại sự kiện, vui lòng không chia sẻ mã này cho người khác</li>
																		<li>Ghế ngồi ngẫu nhiên.</li>
																	</ul>
																	<div class=""QR-scanner"">
																		<img src=""https://api.qrserver.com/v1/create-qr-code/?size=100x100&data={checkinUrl}"" alt=""QR-Ticket"">
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
		
		
							<script src=""js/jquery.min.js""></script>
							<script src=""vendor/bootstrap/js/bootstrap.bundle.min.js""></script>
							<script src=""vendor/OwlCarousel/owl.carousel.js""></script>
							<script src=""vendor/bootstrap-select/dist/js/bootstrap-select.min.js""></script>	
							<script src=""js/custom.js""></script>
							<script src=""js/night-mode.js""></script>
						</body>
					</html>";
                //$@"<img src=""data:image/jpeg;base64,{qrCode}""/>";
            return body;
        }
    }
}
