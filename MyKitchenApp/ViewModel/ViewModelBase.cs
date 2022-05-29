using Microsoft.Toolkit.Mvvm.ComponentModel;
using MyKitchenApp.Interfaces;

namespace MyKitchenApp.ViewModel
{
    public class ViewModelBase : ObservableObject
    {
        #region Properties

        protected readonly ILoggingService _loggingService;

        #endregion

        #region Konstruktoren

        public ViewModelBase()
        {

        }

        public ViewModelBase(ILoggingService loggingService)
        {
            this._loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));

            this.LoadDesignerInstance();
        }

        #endregion

        #region Worker

        protected virtual void LoadDesignerInstance()
        {

        }

        #endregion
    }
}
