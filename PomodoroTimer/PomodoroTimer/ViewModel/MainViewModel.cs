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
        public string WorkingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" + Properties.Settings.Default.workingSounds + ".mp3";
        public string AlarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" + Properties.Settings.Default.alarmSounds + ".mp3";

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
            WorkingSoundsOgg.Stop("workingSounds");
            AlarmSoundsOgg.Play("alarmSounds");
            PomodoroCount++;
            Timer.Stop();
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

        private string FormatTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
