using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using PropertyChanged;

namespace PomodoroTimer.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
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

        public double Volume
        {
            get => Properties.Settings.Default.volume;

            set
            {
                Properties.Settings.Default.volume = value;
                WorkingSoundsOgg.Volume = value / 100;
                AlarmSoundsOgg.Volume = value / 100;
            }
        }

        public string WorkingSounds { get; set; } = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" +
                                                    Properties.Settings.Default.workingSounds + ".mp3";

        public string AlarmSounds { get; set; } = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" +
                                                  Properties.Settings.Default.alarmSounds + ".mp3";

        public MediaPlayer AlarmSoundsOgg;
        public MediaPlayer WorkingSoundsOgg;
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
                if (CurrentPomoStateEnum == PomoStateEnum.Working)
                {
                    Opacity = 0.5;
                }

                Time--;
                return;
            }

            Opacity = 1;
            Timer.Stop();
            ShowStartButton = false;
            ShowPauseButton = false;
            ShowRestartButton = false;
            ShowDoneButton = true;
            if (CurrentPomoStateEnum == PomoStateEnum.Working)
            {
                PomodoroCount++;
                WorkingSoundsOgg.Pause();
                AlarmSoundsOgg.Play();

                CurrentPomoStateEnum = PomoStateEnum.WorkDone;

            }
            else
            {
                AlarmSoundsOgg.Play();
                CurrentPomoStateEnum = PomoStateEnum.RestDone;
            }
        }

        public LinearGradientBrush CurrentLinearGradientBrush { get; set; }

        public string CurrentBackgroundSounds { get; set; }

        [DependsOn(nameof(Time))]
        public string CountdownTimer => FormatTimer();

        public MainViewModel()
        {
            CurrentLinearGradientBrush = WorkingGradient;
            Time = PomodoroDuration;
            CurrentPomoStateEnum = PomoStateEnum.Init;
            CurrentBackgroundSounds = WorkingSounds;
            Opacity = 1;

            WorkingSoundsOgg = new MediaPlayer();
            WorkingSoundsOgg.Open(new Uri(WorkingSounds, UriKind.RelativeOrAbsolute));
            WorkingSoundsOgg.Volume = Volume;
            WorkingSoundsOgg.MediaEnded += (sender, e) =>
            {
                //播放结束后 又重新播放
                WorkingSoundsOgg.Position = TimeSpan.Zero;
            };

            AlarmSoundsOgg = new MediaPlayer();
            AlarmSoundsOgg.Open(new Uri(AlarmSounds, UriKind.RelativeOrAbsolute));
            AlarmSoundsOgg.Volume = Volume;
            Timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            Timer.Tick += TimerTick;
        }

        public double Opacity { get; set; }

        public bool ShowStartButton { get; set; } = true;

        public bool ShowPauseButton { get; set; }

        public bool ShowRestartButton { get; set; }

        public bool ShowDoneButton { get; set; }

        public void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            //
            if (CurrentPomoStateEnum == PomoStateEnum.Init)
            {
                Time = PomodoroDuration;
            }

            ShowStartButton = false;
            ShowPauseButton = true;
            ShowRestartButton = true;
            ShowDoneButton = false;

            WorkingSoundsOgg.Play();
            CurrentPomoStateEnum = PomoStateEnum.Working;
            Timer.Start();
        }

        public void OnPauseButtonClick(object sender, RoutedEventArgs e)
        {
            Opacity = 1;
            ShowStartButton = true;
            ShowPauseButton = false;
            ShowRestartButton = true;
            ShowDoneButton = false;
            WorkingSoundsOgg.Stop();
            CurrentPomoStateEnum = PomoStateEnum.Pause;
            Timer.Stop();
        }

        public void OnRestartButtonClick(object sender, RoutedEventArgs e)
        {
            Opacity = 1;
            ShowStartButton = true;
            ShowPauseButton = false;
            ShowRestartButton = false;
            ShowDoneButton = false;

            Time = PomodoroDuration;
            CurrentPomoStateEnum = PomoStateEnum.Init;
            CurrentLinearGradientBrush = WorkingGradient;
            WorkingSoundsOgg.Stop();
            Timer.Stop();
        }

        public void OnDoneButtonClick(object sender, RoutedEventArgs e)
        {
            Opacity = 1;
            ShowPauseButton = false;
            ShowDoneButton = false;

            if (CurrentPomoStateEnum == PomoStateEnum.WorkDone)
            {
                CurrentLinearGradientBrush = BreakGradient;
                Time = PomodoroBreak;
                CurrentPomoStateEnum = PomoStateEnum.ShortResting;
                if (PomodoroCount % PomodoroLongBreakOccurance == 0)
                {
                    Time = PomodoroLongBreak;
                    CurrentPomoStateEnum = PomoStateEnum.LongResting;
                }
                ShowStartButton = false;
                ShowRestartButton = true;
                Timer.Start();
                return;
            }

            ShowStartButton = true;
            ShowRestartButton = false;
            Time = PomodoroDuration;
            CurrentLinearGradientBrush = WorkingGradient;
        }

        private string FormatTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
