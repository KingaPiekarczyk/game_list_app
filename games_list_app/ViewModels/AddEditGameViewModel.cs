using games_list_app.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace games_list_app.ViewModels
{
    public class AddEditGameViewModel : INotifyPropertyChanged
    {
        //List of available release years
        public List<int> R_Years { get; }

        //Game object which will be added or edited
        private Game _game;
        public Game Game
        {
            get => _game;
            private set
            {
                _game = value;
                OnPropertyChanged(nameof(Game));
            }
        }

        //Constructor for adding new game
        public AddEditGameViewModel()
        {
            //Generate list of years from 1970 to current year
            R_Years = Enumerable.Range(1970, DateTime.Now.Year - 1970 + 1)
                                .Reverse()
                                .ToList();

            //Create new empty game
            Game = new Game();

            //Forward Game property changes to UI
            Game.PropertyChanged += (s, e) =>
            {
                OnPropertyChanged($"Game.{e.PropertyName}");
                CommandManager.InvalidateRequerySuggested(); 
            };

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
        }

        //Constructor for editing existing game
        public AddEditGameViewModel(Game game)
        {
            //Generate list of years
            R_Years = Enumerable.Range(1970, DateTime.Now.Year - 1970 + 1)
                                .Reverse()
                                .ToList();

            //Copy properties from existing game 
            Game = new Game
            {
                Name = game.Name,
                PlayTimeHours = game.PlayTimeHours,
                Platform = game.Platform,
                Genre = game.Genre,
                IsCompleted = game.IsCompleted,
                ReleaseYear = game.ReleaseYear
            };

            //Forward Game property changes to UI
            Game.PropertyChanged += (s, e) =>
            {
                OnPropertyChanged($"Game.{e.PropertyName}");
                CommandManager.InvalidateRequerySuggested(); 
            };

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
        }

        //Save command
        public ICommand SaveCommand { get; }

        //Invoking close event on save
        private void Save()
        {
            CloseRequested?.Invoke(this, true);
        }

        //Event for closing window
        public event EventHandler<bool>? CloseRequested;

        public event PropertyChangedEventHandler? PropertyChanged;

        //Informing UI about property changes
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        private bool CanSave()
        {

            if (Application.Current == null)
                return false;

            var window = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);

            if (window == null)
                return false;

            return !HasValidationErrors(window);
        }

        private bool HasValidationErrors(DependencyObject obj)
        {
            if (Validation.GetHasError(obj))
                return true;

 
            foreach (var child in LogicalTreeHelper.GetChildren(obj))
            {
                if (child is DependencyObject depObj)
                {
                    if (HasValidationErrors(depObj))
                        return true;
                }
            }

            return false;
        }
    }
}
