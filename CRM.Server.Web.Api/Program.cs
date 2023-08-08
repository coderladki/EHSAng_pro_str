using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;

namespace CRM.Server.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            //var directoryPath = Path.GetDirectoryName(exePath);
            //Directory.SetCurrentDirectory(directoryPath);
            //var encryptedPass=   Bharuwa.Common.Utilities.Encryption.StringCipher.Encrypt("6750a5fc15a848179a618d2c950331676750a5fc15a848179a618d2c95033167", "crm!123#");
            //var actualPass=  Bharuwa.Common.Utilities.Encryption.StringCipher.Decrypt("6750a5fc15a848179a618d2c950331676750a5fc15a848179a618d2c95033167", encryptedPass);

            //var encryptedPass=   Bharuwa.Common.Utilities.Encryption.AESEncryption.Encrypt("6750a5fc15a848179a618d2c950331676750a5fc15a848179a618d2c95033167", "purchasing@123");
            //var actualPass=  Bharuwa.Common.Utilities.Encryption.AESEncryption.Decrypt("6750a5fc15a848179a618d2c950331676750a5fc15a848179a618d2c95033167", encryptedPass);

            const string loggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}]<{ThreadId}> [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;

            var logfile = Path.Combine(baseDir, "App_Data", "logs", "log.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .Enrich.With(new ThreadIdEnricher())
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information, loggerTemplate, theme: AnsiConsoleTheme.Literate)
                .WriteTo.File(logfile, LogEventLevel.Debug, loggerTemplate,
                    rollingInterval: RollingInterval.Hour, retainedFileCountLimit: 90)
                .CreateLogger();

            var builder = new ConfigurationBuilder();

            IConfiguration config = builder.Build();
            //
            try
            {

                Log.Information("====================================================================");
                Log.Information($"Application Starts. Version: {System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version}");
                Log.Information($"Application Directory: {AppDomain.CurrentDomain.BaseDirectory}");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.Information("====================================================================\r\n");
                Log.CloseAndFlush();
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(baseDir);
            return Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                            {
                                config.SetBasePath(baseDir);
                            })
                                        .ConfigureWebHostDefaults(webBuilder =>
                                        {
                                            webBuilder.UseStartup<Startup>();
                                        })
                 .UseSerilog();
        }
    }
}