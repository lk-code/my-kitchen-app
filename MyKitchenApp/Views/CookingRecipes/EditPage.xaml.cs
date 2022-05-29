using MyKitchenApp.ViewModel.CookingRecipes;

namespace MyKitchenApp.Views.CookingRecipes;

public partial class EditPage : ContentPage
{
    private readonly EditViewModel ViewModel;

    public EditPage(EditViewModel editViewModel)
    {
        this.BindingContext = this.ViewModel = editViewModel ?? throw new ArgumentNullException(nameof(editViewModel));

        this.InitializeComponent();
    }
}