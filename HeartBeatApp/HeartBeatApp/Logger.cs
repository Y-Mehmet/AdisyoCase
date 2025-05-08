using System.Json.Text;
public sealed class Logger : ILogger
{
    public async Task AddLog(Log log)
    {
        using StreamWriter streamWriter = new StreamWriter(log.Path, true);

        var logObject = new
        {
            timestamp = DateTime.Now.ToString(),
            statusCode = log.StatusCode,
            message = log.Msg,
            url = log.URL
        };

        string jsonLog = JsonSerializer.Serialize(logObject, new JsonSerializerOptions { WriteIndented = true });
        await streamWriter.WriteLineAsync(jsonLog);
        Console.WriteLine(jsonLog);
    }
}
