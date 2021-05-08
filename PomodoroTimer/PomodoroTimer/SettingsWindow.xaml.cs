using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            for (int i = 1; i <= 480; i++)
            {
                pomodoroDurationCombobox.Items.Add(i.ToString());
                pomodoroBreakCombobox.Items.Add(i.ToString());
                pomodoroLongBreakCombobox.Items.Add(i.ToString());
            }

            for(int i = 1; i <= 100; i++)
            {
                pomodoroLongBreakOccuranceCombobox.Items.Add(i.ToString());
            }
            //add population of songs

            pomodoroDurationCombobox.SelectedIndex = Properties.Settings.Default.pomodoroDuration - 1;
            pomodoroBreakCombobox.SelectedIndex = Properties.Settings.Default.pomodoroBreak - 1;
            pomodoroLongBreakCombobox.SelectedIndex = Properties.Settings.Default.pomodoroLongBreak - 1;
            pomodoroLongBreakOccuranceCombobox.SelectedIndex = Properties.Settings.Default.pomodoroLongBreakOccurance - 1;

            //select one of those thingys
            var checkedValueAlarmSounds = alarmSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.Name.Equals(Properties.Settings.Default.alarmSounds));
            if(checkedValueAlarmSounds != null)
            {
                checkedValueAlarmSounds.IsChecked = true;
            }

            var checkedValueWorkingSounds = workingSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.Name.Equals(Properties.Settings.Default.workingSounds));
            if (checkedValueWorkingSounds != null)
            {
                checkedValueWorkingSounds.IsChecked = true;
            }
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.pomodoroDuration = pomodoroDurationCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroBreak = pomodoroBreakCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroLongBreak = pomodoroLongBreakCombobox.SelectedIndex + 1;
            Properties.Settings.Default.pomodoroLongBreakOccurance = pomodoroLongBreakOccuranceCombobox.SelectedIndex + 1;

            var checkedValueAlarmSounds = alarmSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            Properties.Settings.Default.alarmSounds = checkedValueAlarmSounds.Name;

            var checkedValueWorkingSounds = workingSoundsPanel.Children.OfType<RadioButton>()
            .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            Properties.Settings.Default.workingSounds = checkedValueWorkingSounds.Name;

            Properties.Settings.Default.Save();

            MainWindow.PomodoroDuration = Properties.Settings.Default.pomodoroDuration * 60;
            MainWindow.PomodoroBreak = Properties.Settings.Default.pomodoroBreak * 60;
            MainWindow.PomodoroLongBreak = Properties.Settings.Default.pomodoroLongBreak * 60;
            MainWindow.PomodoroLongBreakOccurance = Properties.Settings.Default.pomodoroLongBreakOccurance;
            MainWindow.WorkingSounds = Environment.CurrentDirectory + @"\Assets\Sounds\workingSounds\bgm_" + Properties.Settings.Default.workingSounds + ".mp3";
            MainWindow.AlarmSounds = Environment.CurrentDirectory + @"\Assets\Sounds\alarmSounds\alm_" + Properties.Settings.Default.alarmSounds + ".mp3";
            MainWindow.WorkingSoundsOgg = new MP3Player(MainWindow.WorkingSounds, "workingSounds");
            MainWindow.AlarmSoundsOgg = new MP3Player(MainWindow.AlarmSounds, "alarmSounds");
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
