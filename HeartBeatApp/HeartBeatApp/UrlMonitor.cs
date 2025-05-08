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
    private readonly ILogger _logger;
    private readonly IEmailSender _emailSender;

    private readonly string _baseLogPath = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt";

    public UrlMonitor(URLDataConfig urlDataConfig, ILogger logger, IEmailSender emailSender)
    {
        _urlDataConfig = urlDataConfig;
        _logger = logger;
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
                   
                    Log log = new Log()
                    {
                        StatusCode = (int)response.StatusCode,
                        Status = "Success",
                        Msg = "Success web site is up",
                        URL = _urlDataConfig.URL,
                        Path = _baseLogPath
                    };
                   await _logger.AddLog(log);
                }
                else
                {
                    Log log = new Log()
                    {
                        StatusCode = (int)response.StatusCode,
                        Status = "Failed",
                        Msg = "Failed web site is down",
                        URL = _urlDataConfig.URL,
                        Path = _baseLogPath
                    };
                   await _logger.AddLog(log);
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
                Log log = new Log()
                    {
                        StatusCode = 0,
                        Status = "Failed",
                        Msg = ex.Message,
                        URL = _urlDataConfig.URL,
                        Path = _baseLogPath
                    };
                  await _logger.AddLog(log);
                  string to= _urlDataConfig.Email.To;
                    string subject = $"[Heartbeat Error] {_urlDataConfig.URL}";
                    string body = $@"
                                        An error occurred while checking {_urlDataConfig.URL}.

                                        Time: {DateTime.Now}
                                        Error Message: {ex.Message}
                                        ";
                   await _emailSender.SendEmailAsync(to, subject, body);
            }

            // Her URL için farklı bir delay uygulanacak
            await Task.Delay(_urlDataConfig.Delay * 1000); // Delay süresini saniye cinsinden alıyoruz
        }
    }
}


