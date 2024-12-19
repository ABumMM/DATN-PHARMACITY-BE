using backend.Models;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;

public class SendEmailBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SendEmailBackgroundService> _logger;

    public SendEmailBackgroundService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<SendEmailBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Thực thi công việc gửi email
                await SendWarningMail();

                // Đợi 24 giờ rồi lặp lại
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending email.");
            }
        }
    }

    public async Task SendWarningMail()
    {
        var currentDate = DateTime.Now;

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var _db = scope.ServiceProvider.GetRequiredService<FinalContext>();

            var expiringProducts = await _db.WarehouseProducts
                .Where(wp => wp.ExpirationDate <= currentDate.AddDays(30))
                .Include(wp => wp.IdProductNavigation)
                .Include(wp => wp.IdWarehouseNavigation)
                .ToListAsync();

            if (!expiringProducts.Any())
            {
                _logger.LogInformation("No products are expiring within the next 30 days.");
                return;
            }

            string warningMessage = "Cảnh báo: Các sản phẩm sau đây sắp hết hạn:\n";
            foreach (var wp in expiringProducts)
            {
                warningMessage += $"Sản phẩm: {wp.IdProductNavigation?.Name}, Hạn sử dụng: {wp.ExpirationDate?.ToShortDateString()} tại kho: {wp.IdWarehouseNavigation?.Name}\n";
            }

            await SendEmailAsync("anhnguyen20031609@gmail.com", "Cảnh báo sản phẩm sắp hết hạn!", warningMessage);
        }
    }

    private async Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        try
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Pharmacity rep 1:1", smtpUser));
            emailMessage.To.Add(new MailboxAddress("", recipientEmail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { TextBody = message };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(smtpUser, smtpPassword);
                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email.");
        }
    }
}
