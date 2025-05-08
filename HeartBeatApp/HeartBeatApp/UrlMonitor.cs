using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Mail;


   public class UrlMonitor
{
    private readonly URLDataConfig _urlDataConfig;

    private string _baseLogPath = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt";
    
   private readonly IEmailSender _emailSender;

    public UrlMonitor(URLDataConfig urlDataConfig, IEmailSender emailSender)
    {
        _urlDataConfig = urlDataConfig;
        _emailSender = emailSender;
    }


    public async Task StartAsync()
    {
        using HttpClient httpClient = new HttpClient();
        
        // Sonsuz döngü ile sürekli olarak kontrol yapıyoruz.
        while (true)
        {
            try
            {
                var response = await httpClient.GetAsync(_urlDataConfig.URL);
                if (response.IsSuccessStatusCode)
                {
                    await Logger.Instance.AddLog(_baseLogPath, (int)response.StatusCode, "Success", _urlDataConfig.URL);
                }
                else
                {
                    await Logger.Instance.AddLog(_baseLogPath, (int)response.StatusCode, "Failed", _urlDataConfig.URL);
                    string to= _urlDataConfig.Email.To;
                    string subject = $"[Heartbeat Error] {_urlDataConfig.URL}";
                    string body = $@"
                                        An error occurred while checking {_urlDataConfig.URL}.

                                        Time: {DateTime.Now}
                                        Error Message: {response.StatusCode}
                                        ";
                   await _emailSender.SendEmailAsync(to, subject, body);
                }
            }
            catch (Exception ex)
            {
                await Logger.Instance.AddLog(_baseLogPath, 000, ex.Message, _urlDataConfig.URL);
            }

            // Her URL için farklı bir delay uygulanacak
            await Task.Delay(_urlDataConfig.Delay * 1000); // Delay süresini saniye cinsinden alıyoruz
        }
    }
}


