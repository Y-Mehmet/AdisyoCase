
using System.Text.Json;


   

  public sealed class Logger
{
    
    private Logger() { }

    public static Logger Instance { get; } = new Logger();
    public async Task AddLog(string path, int statusCode, string msg, string URL)
        {
            StreamWriter streamWriter = new StreamWriter(path, true);
            var logObject = new
            {
                timestamp =""+DateTime.Now,
                statusCode =statusCode,
                message = msg,
                url = URL
            };

            string jsonLog = JsonSerializer.Serialize(logObject, new JsonSerializerOptions { WriteIndented = true });          
            await streamWriter.WriteLineAsync(jsonLog);
            streamWriter.Close();
            Console.WriteLine(jsonLog);

        }
}

