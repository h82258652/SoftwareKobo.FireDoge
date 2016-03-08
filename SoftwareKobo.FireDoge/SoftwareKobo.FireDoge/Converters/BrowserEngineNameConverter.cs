using SoftwareKobo.FireDoge.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SoftwareKobo.FireDoge.Converters
{
    public class BrowserEngineNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BrowserEngine)
            {
                var engine = (BrowserEngine)value;
                var attribute = typeof(BrowserEngine).GetField(value.ToString()).GetCustomAttribute(typeof(EnumDisplayNameAttribute)) as EnumDisplayNameAttribute;
                if (attribute == null)
                {
                    return value;
                }
                else
                {
                    return attribute.Name;
                }
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}