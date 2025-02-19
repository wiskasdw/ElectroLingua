using ElectroLingua.Models;
using ElectroLingua.Views;
using System;
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
            // Create the StudentForm and StudentFormViewModel
            var addStudentWindow = new StudentForm();
            var viewModel = new StudentFormViewModel(); // Create StudentFormViewModel

            addStudentWindow.DataContext = viewModel;
            // Open the StudentForm as a dialog
            if (addStudentWindow.ShowDialog() == true)
            {
                // If the user clicked "Save", add the new student to the database
                _dbHelper.AddNewStudent(new Student()
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    Phone = viewModel.Phone,
                });
                LoadStudents();
            }
        }

        private void EditStudent(object parameter)
        {
            if (parameter is Student studentToEdit)
            {
                // Create the StudentForm and StudentFormViewModel
                var editStudentWindow = new StudentForm();
                var viewModel = new StudentFormViewModel(studentToEdit);

                editStudentWindow.DataContext = viewModel;
                // Open the StudentForm as a dialog
                if (editStudentWindow.ShowDialog() == true)
                {
                    _dbHelper.UpdateStudent(new Student()
                    {
                        StudentID = studentToEdit.StudentID,
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Email = viewModel.Email,
                        Phone = viewModel.Phone,
                    });
                    LoadStudents();
                }
            }
        }

        private void DeleteStudent(object parameter)
        {
            if (parameter is Student studentToDelete)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {studentToDelete.FirstName} {studentToDelete.LastName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _dbHelper.DeleteStudent(studentToDelete.StudentID);
                    LoadStudents();
                }
            }
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
}