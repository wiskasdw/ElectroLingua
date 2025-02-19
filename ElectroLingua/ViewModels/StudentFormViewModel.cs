using ElectroLingua.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ElectroLingua.ViewModels;

namespace ElectroLingua.ViewModels
{
    public class StudentFormViewModel : INotifyPropertyChanged
    {
        private Student _student;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public StudentFormViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            _student = new Student(); // Create a new student object
        }

        public StudentFormViewModel(Student student) : this()
        {
            _student = student; // store the student
            FirstName = student.FirstName;
            LastName = student.LastName;
            Email = student.Email;
            Phone = student.Phone;
        }

        private void Save(object parameter)
        {
            // Your Save logic here
            _student.FirstName = FirstName;
            _student.LastName = LastName;
            _student.Email = Email;
            _student.Phone = Phone;

            if (parameter is Window window) // to check if there any window
            {
                window.DialogResult = true; // to say that all changes are saved and we are good to go

                window.Close();
            }
        }

        private void Cancel(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}