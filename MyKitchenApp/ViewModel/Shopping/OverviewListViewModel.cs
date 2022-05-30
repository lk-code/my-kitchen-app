using MyKitchenApp.Interfaces;

namespace MyKitchenApp.ViewModel.Shopping
{
    public class OverviewListViewModel : ViewModelBase
    {
        #region properties

        private readonly IShoppingService _shoppingService;

        #endregion

        #region constructor

        public OverviewListViewModel() : base()
        {

        }

        public OverviewListViewModel(ILoggingService loggingService,
            IShoppingService shoppingService) : base(loggingService)
        {
            this._shoppingService = shoppingService ?? throw new ArgumentNullException(nameof(shoppingService));
        }

        #endregion

        #region logic



        #endregion
    }
}