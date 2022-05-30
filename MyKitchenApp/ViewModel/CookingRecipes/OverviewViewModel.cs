using MyKitchenApp.Interfaces;
using MyKitchenApp.Models.CookingRecipes;
using MyKitchenApp.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyKitchenApp.ViewModel.CookingRecipes
{
    public class OverviewViewModel : ViewModelBase
    {
        #region properties

        private readonly ICookingRecipesService _cookingRecipesService;

        public ObservableCollection<Recipe> Recipes { get; set; }

        private ICommand _initializeCommand;
        public ICommand InitializeCommand => _initializeCommand ?? (_initializeCommand = new RelayCommand((eventArgs) => { _ = this.OnInitializeAsync(); }));

        #endregion

        #region constructor

        public OverviewViewModel() : base()
        {
            this.Recipes = new ObservableCollection<Recipe>();
        }

        public OverviewViewModel(ILoggingService loggingService,
            ICookingRecipesService cookingRecipesService) : base(loggingService)
        {
            this._cookingRecipesService = cookingRecipesService ?? throw new ArgumentNullException(nameof(cookingRecipesService));

            this.Recipes = new ObservableCollection<Recipe>();
        }

        #endregion

        #region logic

        private async Task OnInitializeAsync()
        {
            await this.LoadRecipesAsync();
        }

        private async Task LoadRecipesAsync()
        {
            this.Recipes.Clear();

            List<Recipe> recipes = await this._cookingRecipesService.GetRecipesAsync();
            foreach (Recipe recipe in recipes)
            {
                this.Recipes.Add(recipe);
            }
        }

        #endregion
    }
}