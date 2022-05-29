using MyKitchenApp.ViewModel.Shopping;

namespace MyKitchenApp.Views.Shopping;

public partial class OverviewListPage : ContentPage
{
    private readonly OverviewListViewModel ViewModel;

    public OverviewListPage(OverviewListViewModel overviewViewModel)
    {
        this.BindingContext = this.ViewModel = overviewViewModel ?? throw new ArgumentNullException(nameof(overviewViewModel));

        this.InitializeComponent();
    }
}