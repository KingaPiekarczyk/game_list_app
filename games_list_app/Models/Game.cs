using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace games_list_app.Models
{
    public class Game : INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }

        private string? _name;

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Max 50 characters")]
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string? _genre;

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50, ErrorMessage = "Max 50 characters")]
        public string? Genre
        {
            get => _genre;
            set
            {
                _genre = value;
                OnPropertyChanged(nameof(Genre));
            }
        }

        private string? _platform;

        [Required(ErrorMessage = "Platform is required")]
        [StringLength(50, ErrorMessage = "Max 50 characters")]
        public string? Platform
        {
            get => _platform;
            set
            {
                _platform = value;
                OnPropertyChanged(nameof(Platform));
            }
        }

        private int _releaseYear;

        [Range(1970, 2026, ErrorMessage = "Year must be between 1970 and 2026")]
        public int ReleaseYear
        {
            get => _releaseYear;
            set
            {
                _releaseYear = value;
                OnPropertyChanged(nameof(ReleaseYear));
            }
        }

        private int _playTimeHours;

        [Range(0, 3000, ErrorMessage = "Hours must be between 0 and 3000")]
        public int PlayTimeHours
        {
            get => _playTimeHours;
            set
            {
                _playTimeHours = value;
                OnPropertyChanged(nameof(PlayTimeHours));
            }
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                var property = GetType().GetProperty(columnName);
                if (property == null) return null;

                var value = property.GetValue(this);
                var context = new ValidationContext(this) { MemberName = columnName };
                var results = new System.Collections.Generic.List<ValidationResult>();

                bool valid = Validator.TryValidateProperty(value, context, results);
                return valid ? null : results.First().ErrorMessage;
            }
        }
    }
}
