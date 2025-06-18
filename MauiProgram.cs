using Microsoft.Extensions.Logging;
using uitleen_app.Models;
using uitleen_app.Views;

namespace uitleen_app
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<ApiClient>(serviceProvider => 
                new ApiClient("https://uitleensysteemfirda.nl/api")); // Updated to correct API URL
            
            // Register pages
            builder.Services.AddTransient<LandingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ItemsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
