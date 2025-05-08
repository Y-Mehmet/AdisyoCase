
using HeartBeatApp;
using System.Text.Json;

class Program
{
    static  void Main(string[] args)
    {
       try{
        while(true)
       {
         var json = File.ReadAllText("C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\config.json");
        var config= JsonSerializer.Deserialize<AppConfig>(json);
        List<Task> urlMonitorTasks = new List<Task>();
        foreach (var urlData in config.URLDatas)
        {
           var urlMonitor= new UrlMonitor(urlData);
            Console.WriteLine("Running Program main method");
            
        }
        
       }
       }catch(Exception e)
       {
        Logger.Instance.AddLog("C:\\Github\\AdisyoCase\\HeartBeatApp\\HeartBeatApp\\log.txt", 000, e.Message, "Main");
       }
    }
    
}
public class EmailConfig
{
    public required string SmtpServer { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public required string From { get; set; }
    public string To { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
public class URLDataConfig
{
    public required string URL { get; set; }
    public int Delay { get; set; }
    public required EmailConfig Email { get; set; }
}
public class AppConfig
{
   public required URLDataConfig[] URLDatas { get; set; }


}