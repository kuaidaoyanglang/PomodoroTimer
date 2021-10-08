using PomodoroTimer.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel;
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (r, s) =>
            {
                this.MouseDown += (x, y) =>
                {
                    if (y.LeftButton == MouseButtonState.Pressed)
                    {
                        this.DragMove();
                    }
                };
            };

            MainViewModel = new MainViewModel();
            this.DataContext = MainViewModel;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.OnStartButtonClick(sender,e);
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.OnPauseButtonClick(sender, e);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.OnRestartButtonClick(sender, e);
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.OnDoneButtonClick(sender, e);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Window settingsWindow = new SettingsWindow(this.MainViewModel);
            settingsWindow.ShowDialog();
        }
    }
}
