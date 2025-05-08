using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting HeartBeatApp...");
            string json = File.ReadAllText("C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\config.json");
            AppConfig appConfig = JsonSerializer.Deserialize<AppConfig>(json);

            if (appConfig == null || appConfig.URLDatas == null || appConfig.URLDatas.Length == 0)
            {
                throw new Exception("Configuration is null or no URL data available");
            }

            List<Task> tasks = new List<Task>();

            // Her URL için ayrı bir Task oluşturuluyor
            foreach (var urlData in appConfig.URLDatas)
            {
                UrlMonitor urlMonitor = new UrlMonitor(urlData);
                tasks.Add(urlMonitor.StartAsync());
            }

            // Tüm task'ları paralel olarak çalıştırıyoruz
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            await Logger.Instance.AddLog(
                "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt",
                0,
                ex.Message,
                "Programs.Main");
        }
    }
}


    public class EmailConfig
    {
        public  required  string SmtpServer { get; set; }
        public required  int Port { get; set; }
        public required  bool EnableSsl { get; set; }
        public required string From { get; set; }
        public required  string To { get; set; }
        public required  string UserName { get; set; }
        public required  string Password { get; set; }
    }

    public class URLDataConfig
    {
        public required  string URL { get; set; }
        public required  int Delay { get; set; }
        public required  EmailConfig Email { get; set; }
    }

    public class AppConfig
    {
        public required  URLDataConfig[] URLDatas { get; set; }
    }

