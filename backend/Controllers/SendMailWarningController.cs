using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailWarningController : ControllerBase
    {
        private readonly FinalContext _db;
        private readonly IConfiguration _configuration;

        public SendMailWarningController(FinalContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        // Phương thức để kiểm tra sản phẩm sắp hết hạn và gửi email cảnh báo
        [HttpPost("send-warning")]
        public async Task<IActionResult> SendWarningMail()
        {
            var currentDate = DateTime.Now;

            // Lấy tất cả sản phẩm trong kho có hạn sử dụng dưới 30 ngày
            var expiringProducts = await _db.WarehouseProducts
                .Where(wp => wp.ExpirationDate <= currentDate.AddDays(30))
                .Include(wp => wp.Product)
                .Include(wp => wp.Warehouse)
                .ToListAsync();

            if (!expiringProducts.Any())
            {
                return Ok("Không có sản phẩm nào sắp hết hạn.");
            }

            string warningMessage = "Cảnh báo: Các sản phẩm sau đây sắp hết hạn:\n";
            foreach (var wp in expiringProducts)
            {
                warningMessage += $"Sản phẩm: {wp.Product.Name}, Hạn sử dụng: {wp.ExpirationDate.ToShortDateString()} tại kho: {wp.Warehouse.Name}\n";
            }

            // Gửi email cảnh báo
            await SendEmailAsync("anhnguyen20031609@gmail.com", "Cảnh báo sản phẩm sắp hết hạn, hạn sử dụng còn lại 30 ngày!'", warningMessage);

            return Ok("Email cảnh báo đã được gửi.");
        }

        // Phương thức gửi email sử dụng MailKit và cấu hình từ appsettings.json
        private async Task SendEmailAsync(string recipientEmail, string subject, string message)
        {
            try
            {
                // Lấy thông tin cấu hình từ appsettings.json
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUser = _configuration["EmailSettings:SmtpUser"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Your Company", smtpUser));
                emailMessage.To.Add(new MailboxAddress("", recipientEmail));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder { TextBody = message }; 
                emailMessage.Body = bodyBuilder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    // Kết nối đến máy chủ SMTP
                    await smtpClient.ConnectAsync(smtpServer, smtpPort, false);
                    await smtpClient.AuthenticateAsync(smtpUser, smtpPassword);
                    await smtpClient.SendAsync(emailMessage);
                    await smtpClient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Gửi email thất bại: {ex.Message}");
            }
        }
    }
}
