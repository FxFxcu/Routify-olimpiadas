using Microsoft.Extensions.Logging;
using Routify.Services;
using System.Diagnostics;

namespace Routify
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Debugger.Launch();

            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "routify-log.txt");

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                File.AppendAllText(logPath, $"{DateTime.Now}: UNHANDLED: {e.ExceptionObject}{Environment.NewLine}{Environment.NewLine}");
            };

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Logging.AddProvider(new FileLoggerProvider(logPath));

            const string gatewayBaseUrl = "http://localhost:5032/";

            builder.Services.AddSingleton<TokenStore>();
            builder.Services.AddTransient<JwtAuthHandler>();

            builder.Services.AddHttpClient<AuthService>(client =>
            {
                client.BaseAddress = new Uri(gatewayBaseUrl);
            });

            builder.Services.AddHttpClient<ClienteService>(client =>
            {
                client.BaseAddress = new Uri(gatewayBaseUrl);
            }).AddHttpMessageHandler<JwtAuthHandler>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}