using System;
using System.Globalization;
using System.Windows.Data;

namespace SortingAnimation.View.Converters
{
    /// <summary>
    /// Конвертер из Enum в список значений Enum
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(Array))]
    internal class EnumToItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum)
            {
                return Enum.GetValues(value.GetType());
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}