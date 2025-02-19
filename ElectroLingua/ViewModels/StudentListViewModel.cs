using ElectroLingua.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ElectroLingua.ViewModels
{
    public class StudentListViewModel : INotifyPropertyChanged
    {
        private DatabaseHelper _dbHelper;
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }


        public ICommand AddStudentCommand { get; }
        public ICommand EditStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }

        public StudentListViewModel()
        {
            _dbHelper = new DatabaseHelper();
            LoadStudents();

            AddStudentCommand = new RelayCommand(AddStudent);
            EditStudentCommand = new RelayCommand(EditStudent, CanEditDeleteStudent);
            DeleteStudentCommand = new RelayCommand(DeleteStudent, CanEditDeleteStudent);
        }

        private void LoadStudents()
        {
            var studentsList = _dbHelper.GetAllStudents();
            Students = new ObservableCollection<Student>(studentsList);
        }

        private void AddStudent(object parameter)
        {
            // Implement your Add Student logic here
            MessageBox.Show("Add");
        }

        private void EditStudent(object parameter)
        {
            MessageBox.Show("Edit");
        }

        private void DeleteStudent(object parameter)
        {
            MessageBox.Show("Delete");
        }

        private bool CanEditDeleteStudent(object parameter)
        {
            return parameter != null; // Ensure that an item is selected
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //Simple RelayCommand implementation (you might already have one)
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

}