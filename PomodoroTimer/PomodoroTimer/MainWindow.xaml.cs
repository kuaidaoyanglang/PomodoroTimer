using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using PomodoroTimer.ViewModel;

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

        private void DoneBreakButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.OnDoneBreakButtonClick(sender, e);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Window settingsWindow = new SettingsWindow(this.MainViewModel);
            settingsWindow.ShowDialog();
        }

        private string FormatTimer(int time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
