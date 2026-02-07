using games_list_app.Models;
using games_list_app.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace games_list_app.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //Repository instance to manage list of games and CRUD
        private readonly GameRepository _repository = new();

        //Collection of games displayed in DataGrid
        public ObservableCollection<Game> Games { get; set; } = new();

        //Currently selected game in DataGrid
        public Game? SelectedGame { get; set; }

        //Constructor loads all games from repository
        public MainViewModel()
        {
            foreach (var g in _repository.GetAll())
                Games.Add(g);
        }

        //Add new game to repository
        public void AddGame(Game game)
        {
            try
            {
                _repository.Add(game);
                ReloadGames();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Update existing game in repository
        public void UpdateGame(Game updatedGame)
        {
            try
            {
                if (SelectedGame == null)
                    return;

                _repository.Update(SelectedGame, updatedGame);
                ReloadGames();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Delete selected game from repository
        public void DeleteGame(Game game)
        {
            try
            {
                _repository.Delete(game);
                ReloadGames();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Filter games based on search text
        public void Filter(string text)
        {
            try
            {
                Games.Clear();

                var items = string.IsNullOrWhiteSpace(text)
                    ? _repository.GetAll()
                    : _repository.Search(text);

                foreach (var g in items)
                    Games.Add(g);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Reload list from repository
        private void ReloadGames()
        {
            Games.Clear();
            foreach (var g in _repository.GetAll())
                Games.Add(g);
        }
        //Save to data base
        public void SaveToDatabase()
        {
            _repository.SaveToDataBase();
        }

        //Load fron data base
        public void LoadFromDatabase()
        {
            _repository.LoadFromDataBase();
            ReloadGames();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

