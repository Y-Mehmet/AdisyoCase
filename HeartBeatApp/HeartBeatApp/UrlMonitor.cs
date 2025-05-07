using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeartBeatApp
{
    internal class UrlMonitor
    {
        private readonly URLDataConfig _urlDataConfig;
        private string _baseLogPath = "C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt";
        public UrlMonitor(URLDataConfig urlDataCanfig)
        {
            _urlDataConfig = urlDataCanfig;
        }
        public async Task StartTask()
        {
            Console.WriteLine(" starting task ");
            try
            {
                HttpClient client = new HttpClient();
                using HttpResponseMessage response = await client.GetAsync(_urlDataConfig.URL);
                if (response.IsSuccessStatusCode)
                {
                    int statusCode = (int)response.StatusCode;
                    string msg = "" + response.ReasonPhrase;

                    await AddLog(_baseLogPath, statusCode, msg, _urlDataConfig.URL);
                }
                else
                {
                    int statusCode = (int)response.StatusCode;
                    string msg = "" + response.ReasonPhrase;

                    await AddLog(_baseLogPath, statusCode, msg, _urlDataConfig.URL);
                }
            }catch( HttpRequestException e)
            {
                await AddLog(_baseLogPath,000,e.Message,_urlDataConfig.URL);
            }
        }
        private async Task AddLog(string path, int statusCode, string msg, string URL)
        {
            StreamWriter streamWriter = new StreamWriter(path, true);
            string log = $"[{DateTime.Now}] [{statusCode}]  [{msg}] [{URL}]";
            Console.WriteLine(log);
            await streamWriter.WriteLineAsync(log);
            streamWriter.Close();

        }

    }
}
