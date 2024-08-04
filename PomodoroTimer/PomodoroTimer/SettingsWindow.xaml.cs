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
        private readonly MainViewModel _viewModel;
        public SettingsWindow(MainViewModel viewModel)
        {

            InitializeComponent();
            _viewModel = viewModel;
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

            Volume = _viewModel.Volume;
        }

        public double Volume
        {
            get => _viewModel.Volume;
            set => _viewModel.Volume = value;
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

            _viewModel.PomodoroDuration = Properties.Settings.Default.pomodoroDuration * 60;
            _viewModel.PomodoroBreak = Properties.Settings.Default.pomodoroBreak * 60;
            _viewModel.PomodoroLongBreak = Properties.Settings.Default.pomodoroLongBreak * 60;
            _viewModel.PomodoroLongBreakOccurance = Properties.Settings.Default.pomodoroLongBreakOccurance;
            _viewModel.WorkingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" + Properties.Settings.Default.workingSounds + ".mp3";
            _viewModel.AlarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" + Properties.Settings.Default.alarmSounds + ".mp3";
            _viewModel.Volume = Properties.Settings.Default.volume;

            _viewModel.WorkingSoundsOgg.Volume = Properties.Settings.Default.volume / 100;
            _viewModel.WorkingSoundsOgg.Volume = Properties.Settings.Default.volume / 100;

            //_viewModel.WorkingSoundsOgg = new MP3Player(_viewModel.WorkingSounds, "workingSounds");
            _viewModel.WorkingSoundsOgg.Open(new Uri(_viewModel.WorkingSounds));

            if (_viewModel.CurrentPomoStateEnum == PomoStateEnum.Working)
            {
                _viewModel.WorkingSoundsOgg.Play();
            }

            //_viewModel.AlarmSoundsOgg = new MP3Player(_viewModel.AlarmSounds, "alarmSounds");
            _viewModel.AlarmSoundsOgg.Open(new Uri(_viewModel.WorkingSounds));
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
