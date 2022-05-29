namespace MyKitchenApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Wird nicht benötigt, MAUI erkennt die Route anhand der Ordner.
        //Routing.RegisterRoute("CookingRecipes/Overview", typeof(Views.CookingRecipes.OverviewPage));
        //Routing.RegisterRoute("CookingRecipes/Edit", typeof(Views.CookingRecipes.EditPage));
    }
}
