using System.Net;
using System.Net.Mail;

public class SmtpEmailSender : IEmailSender
{
    private readonly EmailConfig _emailConfig;

    public SmtpEmailSender(EmailConfig emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try{
            using (var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
        {
            client.EnableSsl = _emailConfig.EnableSsl;
            client.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(_emailConfig.To);

            await client.SendMailAsync(mailMessage);
        }
        }catch (Exception ex)
        {
           await Logger.Instance.AddLog(
                "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt",
                0,
                ex.Message,
                "SmtpEmailSender.SendEmailAsync");
        }
    }
}