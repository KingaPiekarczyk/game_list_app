using games_list_app.Models;
using games_list_app.ViewModels;
using System.Windows;

namespace games_list_app.Views
{
    public partial class AddEditGameWindow : Window
    {
        public AddEditGameWindow()
        {
            InitializeComponent();

            var vm = new AddEditGameViewModel();
            DataContext = vm;

            vm.CloseRequested += (_, result) =>
            {
                DialogResult = result;
                Close();
            };
        }

        public AddEditGameWindow(Game game)
        {
            InitializeComponent();

            var vm = new AddEditGameViewModel(game);
            DataContext = vm;

            vm.CloseRequested += (_, result) =>
            {
                DialogResult = result;
                Close();
            };
        }

        public AddEditGameViewModel ViewModel => (AddEditGameViewModel)DataContext;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
