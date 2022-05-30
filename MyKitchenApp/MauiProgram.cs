using Microsoft.Extensions.Configuration;
using MyKitchenApp.Interfaces;
using MyKitchenApp.Services.CookingRecipes;
using MyKitchenApp.Services.Logging;
using MyKitchenApp.Services.Shopping;
using System.Reflection;

namespace MyKitchenApp;

public static class MauiProgram
{
    private static List<Type> _initServiceTypes = new List<Type>();

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

        builder.ConfigureAppConfig();
        builder.ConfigureServices();
        builder.ConfigureViews();

        MauiApp mauiApp = builder.InitializeAndBuild();

        return mauiApp;
    }

    private static MauiApp InitializeAndBuild(this MauiAppBuilder builder)
    {
        // 1. build maui app to generate service-instances
        MauiApp mauiApp = builder.Build();

        // 2. execute initialize-methods
        Task initializeTask = Task.Run(async () =>
        {
            foreach (Type serviceType in _initServiceTypes)
            {
                object service = mauiApp.Services.GetService(serviceType);

                if (service is IInitializeAsync)
                {
                    await ((IInitializeAsync)service).InitializeAsync();
                }

                if (service is IInitialize)
                {
                    ((IInitialize)service).Initialize();
                }
            }
        });
        initializeTask.Wait();

        // 3. return maui app instance
        return mauiApp;
    }

    private static MauiAppBuilder ConfigureViews(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<AppShell>();

        builder.Services.AddTransient<Views.CookingRecipes.OverviewPage>();
        builder.Services.AddTransient<ViewModel.CookingRecipes.OverviewViewModel>();
        builder.Services.AddTransient<Views.CookingRecipes.EditPage>();
        builder.Services.AddTransient<ViewModel.CookingRecipes.EditViewModel>();

        builder.Services.AddTransient<Views.Shopping.OverviewListPage>();
        builder.Services.AddTransient<ViewModel.Shopping.OverviewListViewModel>();

        return builder;
    }

    private static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ILoggingService, LoggingService>();
        _initServiceTypes.Add(typeof(ILoggingService));

        builder.Services.AddSingleton<IShoppingService, ShoppingService>();
        _initServiceTypes.Add(typeof(IShoppingService));

        builder.Services.AddSingleton<ICookingRecipesService, CookingRecipesService>();
        _initServiceTypes.Add(typeof(ICookingRecipesService));

        return builder;
    }

    private static MauiAppBuilder ConfigureAppConfig(this MauiAppBuilder builder)
    {
        Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(MauiProgram)).Assembly;
        string appSettingFileResourceName = $"{typeof(MauiProgram).Namespace}.appsettings.json";
        Stream stream = assembly.GetManifestResourceStream(appSettingFileResourceName);
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);

        return builder;
    }
}
