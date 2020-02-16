using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class converter.
    /// </summary>
    public static class Converter
    {
        private static readonly CultureInfo CultureInfo = CultureInfo.CreateSpecificCulture("en-US");

        /// <summary>
        /// This method converts string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Return string.</returns>
        public static Tuple<bool, string, string> StringConverter(string input)
        {
            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        /// <summary>
        /// This method converts a string to datetime.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Return date.</returns>
        public static Tuple<bool, string, DateTime> DateConverter(string input)
        {
            CultureInfo.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            string message = string.Empty;
            bool success = DateTime.TryParseExact(input, "d", CultureInfo, DateTimeStyles.None, out DateTime result);

            if (!success)
            {
                message = "the date format must be MM/dd/yyyy";
            }

            return new Tuple<bool, string, DateTime>(success, message, result);
        }

        /// <summary>
        /// This method converts a string to short.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Return short value.</returns>
        public static Tuple<bool, string, short> SalaryConverter(string input)
        {
            string message = string.Empty;
            bool success = short.TryParse(input, out short result);

            if (!success)
            {
                message = "integers between -32768 and 32767 were expected";
            }

            return new Tuple<bool, string, short>(success, message, result);
        }

        /// <summary>
        /// This method converts a string to decimal.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Return work rate.</returns>
        public static Tuple<bool, string, decimal> WorkRateConverter(string input)
        {
            string message = string.Empty;
            bool success = decimal.TryParse(input, out decimal result);

            if (!success)
            {
                message = "enter a decimal number";
            }

            return new Tuple<bool, string, decimal>(success, message, result);
        }

        /// <summary>
        /// This method converts a string to char.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>Return gender.</returns>
        public static Tuple<bool, string, char> GenderConverter(string input)
        {
            string message = string.Empty;
            bool success = char.TryParse(input, out char result);

            if (!success)
            {
                message = "one character was expected";
            }

            return new Tuple<bool, string, char>(success, message, result);
        }
    }
}
