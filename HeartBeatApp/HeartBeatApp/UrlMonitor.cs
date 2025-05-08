using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeartBeatApp
{
    public class UrlMonitor
    {
        private readonly URLDataConfig _urlDataConfig;
        private string _baseLogPath = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt";
        public UrlMonitor(URLDataConfig urlDataCanfig)
        {
            _urlDataConfig = urlDataCanfig;
            RecursiveHeartBeat(_urlDataConfig.URL, _urlDataConfig.Delay).GetAwaiter().GetResult();
        }
        
       public  async Task RecursiveHeartBeat(string url, int delay)
    {
        Console.WriteLine("Running RecursiveHeartBeat method is running");
        HttpClient client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            int statusCode = (int)response.StatusCode;
            string msg = "" + response.ReasonPhrase;
            await Logger.Instance.AddLog("C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt", statusCode, msg, url);
        }
        else
        {
            int statusCode = (int)response.StatusCode;
            string msg = "" + response.ReasonPhrase;
            await Logger.Instance.AddLog("C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt", statusCode, msg, url);
        }
        await Task.Delay(delay * 1000); 
        await RequrisifHeartBeat(url, delay); 
    }
        

    }
}
