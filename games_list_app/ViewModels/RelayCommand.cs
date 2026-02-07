using System;
using System.Windows.Input;

namespace games_list_app.ViewModels
{
    public class RelayCommand : ICommand
    {
        //Action to be executed after clicking button
        private readonly Action<object?> _execute;

        //Method to check if button can be activated
        private readonly Func<object?, bool>? _canExecute;

        //Constructor 
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        //Check if command can be executed
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        //Do execute when button is clicked 
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        //Refresh button state
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
