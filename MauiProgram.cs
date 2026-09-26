using Microsoft.Extensions.Logging;
using Routify.App.Services;

namespace Routify
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Base URL del Gateway. Windows/desktop: localhost.
            // Emulador de Android: cambiar a "http://10.0.2.2:5032/"
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