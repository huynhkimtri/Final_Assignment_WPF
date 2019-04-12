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
    class NameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string nameRegex = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
            string errorContent = "Invalid Entry!\nThe name CANNOT include NUMBER or SPECIAL CHARACTERS";

            if (value.ToString() == string.Empty || !Regex.IsMatch(value.ToString(), nameRegex))
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
