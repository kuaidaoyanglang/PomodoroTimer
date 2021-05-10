using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PomodoroTimer.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool c = System.Convert.ToBoolean(parameter);
            bool isShow = System.Convert.ToBoolean(value);
            return isShow ? Visibility.Visible : Visibility.Collapsed;
        }

        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
