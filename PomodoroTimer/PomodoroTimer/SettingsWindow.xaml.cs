using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PomodoroTimer.ViewModel;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public MainViewModel MainViewModel;
        public SettingsWindow(MainViewModel mainViewModel)
        {

            InitializeComponent();
            MainViewModel = mainViewModel;
            for (int i = 1; i <= 480; i++)
            {
                _ = pomodoroDurationCombobox.Items.Add(i.ToString());
                _ = pomodoroBreakCombobox.Items.Add(i.ToString());
                _ = pomodoroLongBreakCombobox.Items.Add(i.ToString());
            }

            for (int i = 1; i <= 100; i++)
            {
                _ = pomodoroLongBreakOccuranceCombobox.Items.Add(i.ToString());
            }
            //add population of songs

            pomodoroDurationCombobox.SelectedIndex = Properties.Settings.Default.pomodoroDuration - 1;
            pomodoroBreakCombobox.SelectedIndex = Properties.Settings.Default.pomodoroBreak - 1;
            pomodoroLongBreakCombobox.SelectedIndex = Properties.Settings.Default.pomodoroLongBreak - 1;
            pomodoroLongBreakOccuranceCombobox.SelectedIndex = Properties.Settings.Default.pomodoroLongBreakOccurance - 1;

            //select one of those thingys
            RadioButton checkedValueAlarmSounds = alarmSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.Name.Equals(Properties.Settings.Default.alarmSounds));
            if (checkedValueAlarmSounds != null)
            {
                checkedValueAlarmSounds.IsChecked = true;
            }

            RadioButton checkedValueWorkingSounds = workingSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.Name.Equals(Properties.Settings.Default.workingSounds));
            if (checkedValueWorkingSounds != null)
            {
                checkedValueWorkingSounds.IsChecked = true;
            }

            volumeSlider.Value = MainViewModel.Volume;
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pomodoroDuration = pomodoroDurationCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroBreak = pomodoroBreakCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroLongBreak = pomodoroLongBreakCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroLongBreakOccurance = pomodoroLongBreakOccuranceCombobox.SelectedIndex + 1;
            Properties.Settings.Default.volume = volumeSlider.Value;

            RadioButton checkedValueAlarmSounds = alarmSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            Properties.Settings.Default.alarmSounds = checkedValueAlarmSounds.Name;

            RadioButton checkedValueWorkingSounds = workingSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            Properties.Settings.Default.workingSounds = checkedValueWorkingSounds.Name;

            Properties.Settings.Default.Save();

            MainViewModel.PomodoroDuration = Properties.Settings.Default.pomodoroDuration * 60;
            MainViewModel.PomodoroBreak = Properties.Settings.Default.pomodoroBreak * 60;
            MainViewModel.PomodoroLongBreak = Properties.Settings.Default.pomodoroLongBreak * 60;
            MainViewModel.PomodoroLongBreakOccurance = Properties.Settings.Default.pomodoroLongBreakOccurance;
            MainViewModel.WorkingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" + Properties.Settings.Default.workingSounds + ".mp3";
            MainViewModel.AlarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" + Properties.Settings.Default.alarmSounds + ".mp3";
            MainViewModel.Volume = Properties.Settings.Default.volume;

            MainViewModel.WorkingSoundsOgg.Volume = Properties.Settings.Default.volume / 100;
            MainViewModel.WorkingSoundsOgg.Volume = Properties.Settings.Default.volume / 100;

            //MainViewModel.WorkingSoundsOgg = new MP3Player(MainViewModel.WorkingSounds, "workingSounds");
            MainViewModel.WorkingSoundsOgg.Open(new Uri(MainViewModel.WorkingSounds));

            if (MainViewModel.CurrentPomoStateEnum == PomoStateEnum.Working)
            {
                MainViewModel.WorkingSoundsOgg.Play();
            }

            //MainViewModel.AlarmSoundsOgg = new MP3Player(MainViewModel.AlarmSounds, "alarmSounds");
            MainViewModel.AlarmSoundsOgg.Open(new Uri(MainViewModel.WorkingSounds));
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the volume level here when the slider's value changes
            MainViewModel.Volume = e.NewValue;
        }
    }
}
