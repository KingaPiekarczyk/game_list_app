using games_list_app.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;

namespace games_list_app.Services
{

    //Collection of all games in app
    public class GameRepository
    {
        //---------------------
        // DataBase SQL lite
        //---------------------

        //Connection to SQL data base
        private readonly string _connString =
            ConfigurationManager.ConnectionStrings["SqlData"].ConnectionString;

        public GameRepository()
        {
            InitDatabase();
        }

        //Initialize database if not existing
        private void InitDatabase()
        {
            using var connection = new SQLiteConnection(_connString);
            connection.Open();
            string sql = @"
            CREATE TABLE IF NOT EXISTS Games(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Platform TEXT NOT NULL,
            Genre TEXT NOT NULL,
            ReleaseYear INTEGER NOT NULL,
            PlayTimeHours INTEGER NOT NULL,
            Completed INTEGER NOT NULL
            );";

            using var cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }

        //Save to database
        public void SaveToDataBase()
        {
            using var connection = new SQLiteConnection(_connString);
            connection.Open();

            //Clear 
            using (var clear = new SQLiteCommand("DELETE FROM Games", connection))
                clear.ExecuteNonQuery();

            foreach (var g in _games)
            {
                string sql = @"INSERT INTO Games
                (Name, Platform, Genre, ReleaseYear, PlayTimeHours, Completed) 
                VALUES(@name, @platform, @genre, @year, @playtimehours, @completed);";

                using var cmd = new SQLiteCommand(sql, connection);
                cmd.Parameters.AddWithValue("@name", g.Name);
                cmd.Parameters.AddWithValue("@platform", g.Platform);
                cmd.Parameters.AddWithValue("@genre", g.Genre);
                cmd.Parameters.AddWithValue("@year", g.ReleaseYear);
                cmd.Parameters.AddWithValue("@playtimehours", g.PlayTimeHours);
                cmd.Parameters.AddWithValue("@completed", g.IsCompleted ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
        }

        //Loading from database
        public void LoadFromDataBase()
        {
            _games.Clear();
            using var connection = new SQLiteConnection(_connString);
            connection.Open();

            string sql = "SELECT Id, Name, Platform, Genre, ReleaseYear, PlayTimeHours, Completed FROM Games";
            using var cmd = new SQLiteCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                _games.Add(new Game
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Platform = reader.GetString(2),
                    Genre = reader.GetString(3),
                    ReleaseYear = reader.GetInt32(4),
                    PlayTimeHours = reader.GetInt32(5),
                    IsCompleted = reader.GetInt32(6) == 1
                });
            }
        }

        //---------------------
        // IN Memory List
        //---------------------

        //Refreshes the list of games
        private readonly ObservableCollection<Game> _games = new();

        //Get all games in list
        public ObservableCollection<Game> GetAll() => _games;

        //Add new game to list
        public void Add(Game game)
        {
            _games.Add(game);
        }

        //Delete game from list
        public void Delete(Game game)
        {
            _games.Remove(game);
        }

        //Update extisting game in list
        public void Update(Game oldGame, Game newGame)
        {
            var index = _games.IndexOf(oldGame);
            //If game exists, update it
            if (index >= 0)
                _games[index] = newGame;
        }

        //Find game by : name, platform, genre or release year
        public IEnumerable<Game> Search(string text)
        {
            return _games.Where(game =>
                game.Name.Contains(text, System.StringComparison.OrdinalIgnoreCase) ||
                game.Platform.Contains(text, System.StringComparison.OrdinalIgnoreCase) ||
                game.Genre.Contains(text, System.StringComparison.OrdinalIgnoreCase) ||
                game.ReleaseYear.ToString().Contains(text)
            );
        }
    }
}
