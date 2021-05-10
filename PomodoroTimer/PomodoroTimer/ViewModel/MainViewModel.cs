using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using PropertyChanged;

namespace PomodoroTimer.ViewModel
{
    public class MainViewModel: INotifyPropertyChanged
    {
        public readonly LinearGradientBrush WorkingGradient = new LinearGradientBrush(
            Color.FromArgb(255, 255, 100, 55),
            Color.FromArgb(255, 251, 68, 116),
            new Point(0, 0),
            new Point(0.55, 0.52));

        public readonly LinearGradientBrush BreakGradient = new LinearGradientBrush(
            Color.FromArgb(255, 23, 232, 217),
            Color.FromArgb(255, 94, 124, 234),
            new Point(0, 0),
            new Point(0.74, 0.73));

        //Saved user settings
        public int PomodoroDuration = Properties.Settings.Default.pomodoroDuration * 60; //25 * 60; 480 max 
        public int PomodoroBreak = Properties.Settings.Default.pomodoroBreak * 60; //5 * 60; 480 max
        public int PomodoroLongBreak = Properties.Settings.Default.pomodoroLongBreak * 60; //15 * 60; 480 max
        public int PomodoroLongBreakOccurance = Properties.Settings.Default.pomodoroLongBreakOccurance; // 100 max
        public string WorkingSounds { get; set; } = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" +
                                                   Properties.Settings.Default.workingSounds + ".mp3";
        

        public string AlarmSounds { get; set; } = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" + Properties.Settings.Default.alarmSounds + ".mp3";

        public MP3Player AlarmSoundsOgg;
        public MP3Player WorkingSoundsOgg;
        public int PomodoroCount;
        public PomoStateEnum CurrentPomoStateEnum { get; set; }

        [AlsoNotifyFor(nameof(CountdownTimer))]
        public int Time { get; set; }
        public DispatcherTimer Timer;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }

        public void TimerTick(object sender, EventArgs e)
        {
            if (Time > 0)
            {
                Time--;
                return;
            }
            Timer.Stop();
            if (CurrentPomoStateEnum == PomoStateEnum.Working)
            {
                PomodoroCount++;
                WorkingSoundsOgg.Stop("workingSounds");
                AlarmSoundsOgg.Play("alarmSounds");
                CurrentPomoStateEnum = PomoStateEnum.WorkDone;
            }
            else
            {
                CurrentPomoStateEnum = PomoStateEnum.RestDone;
            }
        }

        public LinearGradientBrush CurrentLinearGradientBrush { get; set; }

        [DependsOn(nameof(Time))]
        public string CountdownTimer => FormatTimer();

        public bool IsChanged { get; set; }

        public MainViewModel()
        {
            this.CurrentLinearGradientBrush = WorkingGradient;
            Time = PomodoroDuration;
            CurrentPomoStateEnum = PomoStateEnum.Init;

            WorkingSoundsOgg = new MP3Player(WorkingSounds, "workingSounds");
            AlarmSoundsOgg = new MP3Player(AlarmSounds, "alarmSounds");
            WorkingSoundsOgg.Volume("workingSounds", 1000);
            AlarmSoundsOgg.Volume("alarmSounds", 1000);

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += TimerTick;
        }

        public bool ShowStartButton { get; set; } = true;

        public bool ShowPauseButton { get; set; } = false;

        public bool ShowRestartButton { get; set; } = false;

        public bool ShowDoneBreakButton { get; set; } = false;

        public void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            Timer.Start();
        }

        public void OnPauseButtonClick(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        public void OnRestartButtonClick(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        public void OnDoneBreakButtonClick(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        private string FormatTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
