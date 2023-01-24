using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Shared.Utils.Logger;

public static class LoggerFactory
{
    public static void CreateAndUseLogger(this ConfigureHostBuilder host, IConfiguration configuration)
    {
        CreateLogger("Voice_Server", configuration);
        host.UseSerilog();
    }

    private static ILogger CreateLogger(string name, IConfiguration configuration)
        => Logger(name, true, configuration);

    private static ILogger Logger(string name, IConfiguration configuration = null)
    {
        var logger = Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithProperty("ApplicationContext", name)
            .Enrich.FromLogContext()
            .WriteTo.ColoredConsole()
            .WriteTo.RollingFile("logs/{Year}/{Month}/{Day}/{Hour}/log.txt")
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        return logger;
    }

    private static ILogger Logger(string name, bool useSeq, IConfiguration configuration = null)
    {
        var today = DateTime.Now;
        var logger = Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithProperty("ApplicationContext", name)
            .Enrich.FromLogContext()
            .WriteTo.Seq(configuration["SeqUrl"] ?? "http://localhost:5341")
            .WriteTo.Console()
            .WriteTo.RollingFile($"logs/{today.Year}/{today.Month}/{today.Day}/log.txt")
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        return logger;
    }
}