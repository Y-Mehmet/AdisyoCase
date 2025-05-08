using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;


   public class UrlMonitor
{
    private readonly URLDataConfig _urlDataConfig;
    private string _baseLogPath = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt";
    
    public UrlMonitor(URLDataConfig urlDataConfig)
    {
        _urlDataConfig = urlDataConfig;
        Console.WriteLine($"Monitoring URL: {_urlDataConfig.URL} with delay: {_urlDataConfig.Delay} seconds");
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


