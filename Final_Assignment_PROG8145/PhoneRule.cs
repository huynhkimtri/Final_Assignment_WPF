using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Final_Assignment_PROG8145
{
    class PhoneRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phoneRegex = @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$";
            string errorConter = "Invalid Entry!\nPhone number format like \n" +
                    "(012)3456789\n" +
                    "(012)345-6789\n" +
                    "0123456789\n" +
                    "012-345-6789";

            if (value.ToString() == string.Empty || !Regex.IsMatch(value.ToString(), phoneRegex))
            {
                return new ValidationResult(false, errorConter);
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }

    }
}
