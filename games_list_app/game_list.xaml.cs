using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using games_list_app.ViewModels;
using games_list_app.Views;
using games_list_app.Models;
using System.Windows.Input;

namespace games_list_app
{
    public partial class Window1 : Window
    {
        public MainViewModel ViewModel { get; }

        public Window1()
        {
            InitializeComponent();
            Loaded += WindowLoaded;
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = FindVisualChild<ScrollViewer>(GamesDataGrid);

            if (scrollViewer == null)
                return;

            ExternalScrollBar.Minimum = 0;
            ExternalScrollBar.Maximum = scrollViewer.ScrollableHeight;

            ExternalScrollBar.ValueChanged += (s, ev) =>
            {
                scrollViewer.ScrollToVerticalOffset(ExternalScrollBar.Value);
            };

            scrollViewer.ScrollChanged += (s, ev) =>
            {
                ExternalScrollBar.Maximum = scrollViewer.ScrollableHeight;
                ExternalScrollBar.Value = scrollViewer.VerticalOffset;
            };
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T correctlyTyped)
                    return correctlyTyped;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
        private void OnlyNumbers(object sender , TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void ValidateYear(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textb && int.TryParse (textb.Text, out int value))
            {
                if (value < 1970) textb.Text = "1970";
                if (value > 2016) textb.Text = "2016";
            }
        }
        private void ValidateHours(object sender, RoutedEventArgs e)
        {
            if ( sender is TextBox textb && int.TryParse(textb.Text, out int value))
            {
                if( value<0) textb.Text = "0";
                if (value > 3000) textb.Text = "3000";
            }
        }

        private void Edit_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Game? selected = GamesDataGrid.SelectedItem as Game;

            AddEditGameWindow window = selected == null
                ? new AddEditGameWindow()        
                : new AddEditGameWindow(selected); 

            if (window.ShowDialog() == true)
            {
                if (selected == null)
                    ViewModel.AddGame(window.ViewModel.Game);
                else
                    ViewModel.UpdateGame(window.ViewModel.Game);
            }
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GamesDataGrid.SelectedItem is Game selectedGame)
            {
                ViewModel.DeleteGame(selectedGame);
            }
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveToDatabase();
        }
        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadFromDatabase();
        }


        private void Search_bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Filter(Search_bar.Text);
        }
    }
}

