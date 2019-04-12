using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Final_Assignment_PROG8145
{
    class AgeRule : ValidationRule
    {
        public Int16 Min { get; set; } = 1;
        public Int16 Max { get; set; } = 99;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Int16 age = 0;
            string errorContent = $"Invalid entry!\nAge should be between {Min} and {Max}";

            if (!Int16.TryParse((String)value, out age))
            {
                return new ValidationResult(false, errorContent);
            }
            else if (age <= Min || age >= Max)
            {
                return new ValidationResult(false, errorContent);
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
