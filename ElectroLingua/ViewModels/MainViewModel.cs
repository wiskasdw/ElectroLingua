using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ElectroLingua.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private StudentListViewModel _studentListViewModel;

        public StudentListViewModel StudentListViewModel
        {
            get { return _studentListViewModel; }
            set
            {
                _studentListViewModel = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            StudentListViewModel = new StudentListViewModel();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}