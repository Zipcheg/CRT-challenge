using System.Collections.Generic;
using System.ComponentModel;

namespace AppCopyFile
{
    class ViewModelsResolver : IViewModelsResolver
    {
        private readonly Dictionary<string, INotifyPropertyChanged> vmsResolver =
            new Dictionary<string, INotifyPropertyChanged>();

        public ViewModelsResolver()
        {
            vmsResolver.Add("MainViewModel", new MainViewModel());
            vmsResolver.Add("CopyProcessViewModel", new CopyProcessViewModel());            
        }

        public INotifyPropertyChanged GetViewModelInstance(string viewModelName)
        {
            return vmsResolver[viewModelName];
        }
    }
}
