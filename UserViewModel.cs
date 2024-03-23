using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public class UserViewModel : BaseBindable
    {
        private User _user;
        private bool _isProcessing;

        public UserViewModel()
        {
            User = new User();
            ProceedCommand = new RelayCommand<object>(async _ => await OnProceedClicked(), _ => CanExecute());

        }

        public User User
        {
            get => _user;
            set
            {
                if (SetProperty(ref _user, value))
                {
                    User.PropertyChanged += OnUserPropertyChanged;
                }
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetProperty(ref _isProcessing, value);
        }

        public ICommand ProceedCommand { get; }

        private async Task OnProceedClicked()
        {
            IsProcessing = true;

            try
            {
                if (User.Age < 0 || User.Age > 135)
                {
                    MessageBox.Show("Invalid age. Age cannot be less than 0 or greater than or equal to 135.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                await OnUpdateAsync();
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private async Task OnUpdateAsync()
        {
            await Task.Delay(1000); // Simulating asynchronous operation

            if (User.CalculateIsBirthdayToday(User.BirthDate))
            {
                ShowBirthdayWindow();
            }

            ShowUserInfo();
        }

        private void ShowBirthdayWindow()
        {
            var birthdayViewModel = new BirthdayViewModel(User);
            var birthdayWindow = new BirthdayWindow { DataContext = birthdayViewModel };
            birthdayWindow.Show();
        }

        private void ShowUserInfo()
        {
            MessageBox.Show(
                $"First Name: {User.FirstName}\n" +
                $"Last Name: {User.LastName}\n" +
                $"Email Address: {User.EmailAddress}\n" +
                $"Birth Date: {User.BirthDate.ToShortDateString()}\n" +
                $"Age: {User.FormattedAge}\n" +
                $"Is Adult: {User.IsAdult}\n" +
                $"Sun Sign: {User.SunSign}\n" +
                $"Chinese Sign: {User.ChineseSign}",
                "User Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnUserPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(User.BirthDate))
            {
                ((RelayCommand<object>)ProceedCommand).RaiseCanExecuteChanged();
            }
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private bool CanExecute()
        {
            return !IsProcessing &&
                   !string.IsNullOrWhiteSpace(User.FirstName) &&
                   !string.IsNullOrWhiteSpace(User.LastName) &&
                   !string.IsNullOrWhiteSpace(User.EmailAddress);
        }
/// <summary>
/// Sets property value and raises PropertyChanged event if value has changed.
/// </summary>
protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
