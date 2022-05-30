using MyKitchenApp.Interfaces;

namespace MyKitchenApp.ViewModel.CookingRecipes
{
    public class EditViewModel : ViewModelBase
    {
        #region properties

        private readonly ICookingRecipesService _cookingRecipesService;

        #endregion

        #region constructor

        public EditViewModel() : base()
        {

        }

        public EditViewModel(ILoggingService loggingService,
            ICookingRecipesService cookingRecipesService) : base(loggingService)
        {
            this._cookingRecipesService = cookingRecipesService ?? throw new ArgumentNullException(nameof(cookingRecipesService));
        }

        #endregion

        #region logic



        #endregion
    }
}