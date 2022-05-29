using MyKitchenApp.ViewModel.CookingRecipes;

namespace MyKitchenApp.Views.CookingRecipes;

public partial class OverviewPage : ContentPage
{
    private readonly OverviewViewModel ViewModel;

    public OverviewPage(OverviewViewModel overviewViewModel)
    {
        this.BindingContext = this.ViewModel = overviewViewModel ?? throw new ArgumentNullException(nameof(overviewViewModel));

        this.InitializeComponent();
    }
}