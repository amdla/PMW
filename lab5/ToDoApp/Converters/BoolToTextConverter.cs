using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ToDoApp.Converters
{
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isEditing && parameter is string options)
            {
                var texts = options.Split(',');
                return isEditing ? texts[1] : texts[0];
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}