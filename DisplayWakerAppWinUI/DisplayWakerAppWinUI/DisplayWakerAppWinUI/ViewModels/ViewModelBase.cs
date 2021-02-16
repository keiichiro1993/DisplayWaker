using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DisplayWakerAppWinUI.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _IsLoading = false;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                RaisePropertyChanged();
            }
        }

        private string _LoadingMessage = "Please wait...";
        public string LoadingMessage
        {
            get { return _LoadingMessage; }
            set
            {
                _LoadingMessage = value;
                RaisePropertyChanged();
            }
        }
    }
}
