using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LinearGradientBrush _workingGradient = new LinearGradientBrush(
            Color.FromArgb(255, 255, 100, 55),
            Color.FromArgb(255, 251, 68, 116),
            new Point(0, 0),
            new Point(0.55, 0.52));

        private readonly LinearGradientBrush _breakGradient = new LinearGradientBrush(
            Color.FromArgb(255, 23, 232, 217),
            Color.FromArgb(255, 94, 124, 234),
            new Point(0, 0),
            new Point(0.74, 0.73));

        //Saved user settings
        public static int PomodoroDuration = Properties.Settings.Default.pomodoroDuration * 60; //25 * 60; 480 max 
        public static int PomodoroBreak = Properties.Settings.Default.pomodoroBreak * 60; //5 * 60; 480 max
        public static int PomodoroLongBreak = Properties.Settings.Default.pomodoroLongBreak * 60; //15 * 60; 480 max
        public static int PomodoroLongBreakOccurance = Properties.Settings.Default.pomodoroLongBreakOccurance; // 100 max
        public static string WorkingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" + Properties.Settings.Default.workingSounds + ".mp3";
        public static string AlarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" + Properties.Settings.Default.alarmSounds + ".mp3";

        public static MP3Player AlarmSoundsOgg;
        public static MP3Player WorkingSoundsOgg;
        private int _pomodoroCount;
        private startStopRestartEnum _startStopBool = startStopRestartEnum.start;
        private int _time;
        private readonly DispatcherTimer _timer;

        private enum startStopRestartEnum
        {
            start,
            stop,
            restart,
            resume,
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Background = _workingGradient;
            _time = PomodoroDuration;
            CountdownTimer.Content = FormatTimer(_time);

            WorkingSoundsOgg = new MP3Player(WorkingSounds, "workingSounds");
            AlarmSoundsOgg = new MP3Player(AlarmSounds, "alarmSounds");
            WorkingSoundsOgg.Volume("workingSounds", 1000);
            AlarmSoundsOgg.Volume("alarmSounds", 1000);

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += TimerTick;
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (_time > 0)
            {
                _time--;
                CountdownTimer.Content = FormatTimer(_time);
            }
            else
            {
                WorkingSoundsOgg.Stop("workingSounds");
                AlarmSoundsOgg.Play("alarmSounds");

                _pomodoroCount++;
                _timer.Stop();
                StartPauseButton.Content = "OK";
                _startStopBool = startStopRestartEnum.restart;
                RestartButton.Visibility = Visibility.Collapsed;
            }
        }

        private void StartPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if(_startStopBool == startStopRestartEnum.start)
            {
                if(_pomodoroCount < PomodoroLongBreakOccurance * 2 + 1)
                {
                    if(_pomodoroCount % 2 == 0)
                    {
                        this.Background = _workingGradient;
                        StartPauseButton.Visibility = Visibility.Visible;
                        RestartButton.Visibility = Visibility.Visible;
                        DoneBreakButton.Visibility = Visibility.Collapsed;
                        _time = PomodoroDuration;
                    }
                    else
                    {
                        this.Background = _breakGradient;
                        StartPauseButton.Visibility = Visibility.Collapsed;
                        RestartButton.Visibility = Visibility.Collapsed;
                        DoneBreakButton.Visibility = Visibility.Visible;
                        _time = PomodoroBreak;
                    }
                }
                else
                {
                    this.Background = _breakGradient;
                    StartPauseButton.Visibility = Visibility.Collapsed;
                    RestartButton.Visibility = Visibility.Collapsed;
                    DoneBreakButton.Visibility = Visibility.Visible;
                    _time = PomodoroLongBreak;
                    _pomodoroCount = -1;
                }

                WorkingSoundsOgg.Play("workingSounds");

                _timer.Start();
                StartPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-pause-big.png")));
                _startStopBool = startStopRestartEnum.stop;
            }
            else if (_startStopBool == startStopRestartEnum.restart)
            {
                AlarmSoundsOgg.Stop("alarmSounds");
                StartPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-start-big.png")));
                _startStopBool = startStopRestartEnum.start;
                RestartButton.Visibility = Visibility.Collapsed;
            }
            else if (_startStopBool==startStopRestartEnum.stop)
            {
                WorkingSoundsOgg.Stop("workingSounds");
                AlarmSoundsOgg.Stop("alarmSounds");

                _timer.Stop();
                StartPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-start-big.png")));
                _startStopBool = startStopRestartEnum.resume;
                RestartButton.Visibility = Visibility.Collapsed;
            }
            else if(_startStopBool == startStopRestartEnum.resume)
            {
                WorkingSoundsOgg.Play("workingSounds");
                _timer.Start();
                StartPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-pause-big.png")));
                _startStopBool = startStopRestartEnum.stop;
                RestartButton.Visibility = Visibility.Visible;
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            if(_startStopBool == startStopRestartEnum.stop)
            {
                AlarmSoundsOgg.Stop("alarmSounds");
                WorkingSoundsOgg.Stop("workingSounds");

                _pomodoroCount = 0;
                _timer.Stop();
                CountdownTimer.Content = FormatTimer(PomodoroDuration);

                _startStopBool = startStopRestartEnum.start;
                StartPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-start-big.png")));
                RestartButton.Visibility = Visibility.Collapsed;
            }
        }

        private void DoneBreakButton_Click(object sender, RoutedEventArgs e)
        {
            AlarmSoundsOgg.Stop("alarmSounds");
            WorkingSoundsOgg.Stop("workingSounds");
            _timer.Stop();
            CountdownTimer.Content = FormatTimer(PomodoroDuration);

            _startStopBool = startStopRestartEnum.start;
            StartPauseButton.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Assets/Buttons/timer-start-big.png")));
            DoneBreakButton.Visibility = Visibility.Collapsed;
            StartPauseButton.Visibility = Visibility.Visible;

            if (_time > 0)
            {
                _pomodoroCount++;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Window settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private static string FormatTimer(int time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
