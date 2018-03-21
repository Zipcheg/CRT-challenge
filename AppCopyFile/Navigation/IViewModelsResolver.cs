using System.ComponentModel;

namespace AppCopyFile
{
    public interface IViewModelsResolver
    {
        INotifyPropertyChanged GetViewModelInstance(string viewModelName);
    }

}
