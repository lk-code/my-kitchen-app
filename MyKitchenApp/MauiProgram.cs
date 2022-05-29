using LKCode.Helper.Extensions;
using MyKitchenApp.Interfaces;
using MyKitchenApp.Services.Logging;

namespace MyKitchenApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        // fonts
        builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // views and viewmodels
        builder.Services.AddTransient<AppShell>();

        builder.Services.AddTransient<Views.CookingRecipes.OverviewPage>();
        builder.Services.AddTransient<ViewModel.CookingRecipes.OverviewViewModel>();
        builder.Services.AddTransient<Views.CookingRecipes.EditPage>();
        builder.Services.AddTransient<ViewModel.CookingRecipes.EditViewModel>();

        builder.Services.AddTransient<Views.Shopping.OverviewListPage>();
        builder.Services.AddTransient<ViewModel.Shopping.OverviewListViewModel>();

        // services
        builder.Services.AddLKCodeConfig();
        builder.Services.AddSingleton<ILoggingService, LoggingService>();

        return builder.Build();
    }
}
