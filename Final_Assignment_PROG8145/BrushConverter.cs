using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Final_Assignment_PROG8145
{
    class BrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Customer)
            {
                Customer customer = (Customer)value;
                if (customer.Time.Equals("8AM")) return Brushes.DarkRed;
                else if (customer.Time.Equals("9AM")) return Brushes.DarkRed;
                else if (customer.Time.Equals("10AM")) return Brushes.DarkRed;
                else if (customer.Time.Equals("11AM")) return Brushes.DarkRed;
                else if (customer.Time.Equals("12PM")) return Brushes.DarkRed;
                else if (customer.Time.Equals("2PM")) return Brushes.Black;
                else if (customer.Time.Equals("3PM")) return Brushes.Black;
                else if (customer.Time.Equals("4PM")) return Brushes.Black;
                else if (customer.Time.Equals("5PM")) return Brushes.Black;
                else if (customer.Time.Equals("6PM")) return Brushes.Black;
                else return Brushes.Black;
            }
            else return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
