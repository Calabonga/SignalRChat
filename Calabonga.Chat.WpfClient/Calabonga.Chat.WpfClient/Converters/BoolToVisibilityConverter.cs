using System;
using System.Windows;
using System.Windows.Data;

namespace Calabonga.Chat.WpfClient.Converters
{
    /// <summary>
    /// Converts <see cref="bool"/> values to <see cref="Visibility"/> value
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {

        #region IValueConverter Members 
        /// <summary> 
        /// Converts a value. 
        /// </summary> 
        /// <param name="value">The value produced by the binding source.</param> 
        /// <param name="targetType">The type of the binding target property.</param> 
        /// <param name="parameter">The converter parameter to use.</param> 
        /// <param name="culture">The culture to use in the converter.</param> 
        /// <returns> 
        /// A converted value. If the method returns null, the valid null value is used. 
        /// </returns> 
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is not bool val || targetType != typeof(Visibility))
            {
                return Visibility.Collapsed;
            }

            if (parameter is string)
            {
                val = !val;
            }

            if (val)
            {
                return Visibility.Visible;
            }

            return parameter is Visibility ? parameter : Visibility.Collapsed;

        }

        /// <summary> 
        /// Converts a value. 
        /// </summary> 
        /// <param name="value">The value that is produced by the binding target.</param> 
        /// <param name="targetType">The type to convert to.</param> 
        /// <param name="parameter">The converter parameter to use.</param> 
        /// <param name="culture">The culture to use in the converter.</param> 
        /// <returns> 
        /// A converted value.
        /// If the method returns null, the valid null value is used. 
        /// </returns> 
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility && targetType == typeof(bool))
            {
                Visibility val = (Visibility)value;
                if (val == Visibility.Visible)
                    return true;
                return false;
            }
            throw new ArgumentException("Invalid argument/return type. Expected argument: Visibility and return type: bool");
        }
        #endregion
    }
}
