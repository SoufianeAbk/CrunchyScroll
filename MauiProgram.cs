using CrunchyScroll.Services;
using CrunchyScroll.ViewModels;
using CrunchyScroll.Views;
using Microsoft.Extensions.Logging;

namespace CrunchyScroll
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

            // Register Services
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<OrderService>();

            // Register ViewModels
            builder.Services.AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<ProductDetailViewModel>();
            builder.Services.AddTransient<OrderViewModel>();
            builder.Services.AddTransient<OrderHistoryViewModel>();

            // Register Views
            builder.Services.AddTransient<ProductsPage>();
            builder.Services.AddTransient<ProductDetailPage>();
            builder.Services.AddTransient<OrderPage>();
            builder.Services.AddTransient<OrderHistoryPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
