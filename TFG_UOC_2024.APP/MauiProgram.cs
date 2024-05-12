using AutoMapper;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TFG_UOC_2024.APP.Interfaces;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.APP.Views;
using Syncfusion.Maui.Core.Hosting;
using TFG_UOC_2024.APP.Helper;

namespace TFG_UOC_2024.APP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            /*builder.Services.AddScoped<IPlatformHttpMessageHandler>(_ =>
            {
#if ANDROID
                return new AndroidHttpMessageHandler();
#else
                    return new Platforms.iOS.IosHttpMessageHandler();
#endif

            });*/

            builder.ConfigureSyncfusionCore();
            builder.Services.AddCustomApiHttpClient();
            builder.Services.AddSingleton<IMapper, Mapper>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IRecipeService, RecipeService>();
            builder.Services.AddSingleton<IMenuService, MenuService>();

            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<CategoryViewModel>();
            builder.Services.AddSingleton<IngredientsViewModel>();
            builder.Services.AddSingleton<MenuViewModel>();
            builder.Services.AddSingleton<RecipeDetailViewModel>();
            builder.Services.AddSingleton<SearchRecipesViewModel>();
            builder.Services.AddSingleton<UserProfileViewModel>();
            builder.Services.AddSingleton<FavouriteViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SignUpPage>();
            builder.Services.AddTransient<CategoryView>();
            builder.Services.AddTransient<IngredientsView>();
            builder.Services.AddTransient<MenuView>();
            builder.Services.AddTransient<RecipeDetail>();
            builder.Services.AddTransient<SearchRecipesView>();
            builder.Services.AddTransient<UserProfileView>();
            builder.Services.AddTransient<FavouriteView>();



#if DEBUG
            builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            ServiceHelper.Initialize(app.Services);
            return app;
        }

        public static IServiceCollection AddCustomApiHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("Client", httpClient =>
            {
                var baseAddress =
                        DeviceInfo.Platform == DevicePlatform.Android
                            ? "http://10.0.2.2:5010"
                            : "http://192.168.1.33:5010";

                httpClient.BaseAddress = new Uri(baseAddress);
            }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            return services;
        }
    }
}
