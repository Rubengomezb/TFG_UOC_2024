﻿using Microsoft.Extensions.Logging;
using TFG_UOC_2024.APP.Interfaces;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;

namespace TFG_UOC_2024.APP
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

            builder.Services.AddCustomApiHttpClient();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ApplicationDetailsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static IServiceCollection AddCustomApiHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("Client", httpClient =>
            {
                var baseAddress =
                        DeviceInfo.Platform == DevicePlatform.Android
                            ? "https://10.0.2.2:44318"
                            : "https://localhost:44318";

                httpClient.BaseAddress = new Uri(baseAddress);
            });

            return services;
        }
    }
}
