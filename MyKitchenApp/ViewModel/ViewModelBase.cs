using Microsoft.Toolkit.Mvvm.ComponentModel;
using MyKitchenApp.Interfaces;

namespace MyKitchenApp.ViewModel
{
    public class ViewModelBase : ObservableObject
    {
        #region properties

        protected readonly ILoggingService _loggingService;

        #endregion

        #region constructor

        public ViewModelBase()
        {

        }

        public ViewModelBase(ILoggingService loggingService)
        {
            this._loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));

            this.LoadDesignerInstance();
        }

        #endregion

        #region logic

        protected virtual void LoadDesignerInstance()
        {

        }

        #endregion
    }
}
