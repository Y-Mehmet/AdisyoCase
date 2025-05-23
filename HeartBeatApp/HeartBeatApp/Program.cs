﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
public class Program
{
    private static string _baseLogPath = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt";
    public static string _configPath = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\config.json";
    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting HeartBeatApp...");
            string json = File.ReadAllText(_configPath);
            AppConfig appConfig = JsonSerializer.Deserialize<AppConfig>(json);

            if (appConfig == null || appConfig.URLDatas == null || appConfig.URLDatas.Length == 0)
            {
                throw new Exception("Configuration is null or no URL data available");
            }

            List<Task> tasks = new List<Task>();

           
            foreach (var urlData in appConfig.URLDatas)
            {
                
                ILogger logger = new Logger();
                IEmailSender emailSender = new SmtpEmailSender(urlData.Email,logger);
                ILogger logger2 = new Logger();
                UrlMonitor urlMonitor = new UrlMonitor(urlData,logger2, emailSender);
                tasks.Add(urlMonitor.StartAsync());
            }

           
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
           
                Log log= new Log()
                {
                    StatusCode = 0,
                    Status = "Failed",
                    Msg = ex.Message,
                    URL = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\Programs.Main",
                    Path = _baseLogPath
                };
                Console.WriteLine(log.ToString());
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
    public class Log
{
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Msg { get; set; }
        public string URL { get; set; }
        public string Path { get; set; } = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt";
}
