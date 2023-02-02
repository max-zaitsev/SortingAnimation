using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SortingAnimation.View.Converters
{
    /// <summary>
    /// Конвертер из Enum и входящего параметра в bool
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool))]
    internal class EnumToBooleanConverter : IValueConverter
    {
        private const char Separator = '|';

        public string DefaultParameter { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                parameter = DefaultParameter;
            }

            return parameter != null && parameter.ToString().Split(Separator).Contains(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var firstParameter = parameter.ToString().Split(Separator).First();

            return (bool) value ? Enum.Parse(targetType, firstParameter) :
                DefaultParameter != null ? Enum.Parse(targetType, DefaultParameter) : default(Enum);
        }
    }
}