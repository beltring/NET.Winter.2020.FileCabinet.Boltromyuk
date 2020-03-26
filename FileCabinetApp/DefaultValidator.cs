using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp
{
    /// <summary>
    /// This class provides data validation.
    /// </summary>
    internal class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Default validate parameters.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (parameters == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} can't be null.");
            }

            if (parameters.FirstName == null)
            {
                throw new ArgumentNullException($"The {nameof(parameters.FirstName)} parameter must not be null");
            }

            if (parameters.LastName == null)
            {
                throw new ArgumentNullException($"The {nameof(parameters.LastName)} parameter must not be null");
            }

            if (parameters.FirstName.Length < 2 || parameters.FirstName.Length > 60)
            {
                throw new ArgumentException($"The length of {nameof(parameters.FirstName)} must be between 2 and 60 characters");
            }

            if (parameters.LastName.Length < 2 || parameters.LastName.Length > 60)
            {
                throw new ArgumentException($"The length of {nameof(parameters.LastName)} must be between 2 and 60 characters");
            }

            if (parameters.DateOfBirth.Year < 1950 || (parameters.DateOfBirth > currentDate))
            {
                throw new ArgumentException("Minimum date of birth 01-Jan-1950 maximum current date");
            }

            if (parameters.Salary < 100 || parameters.Salary > 10000)
            {
                throw new ArgumentException($"Invalid value, the {nameof(parameters.Salary)} value must be between 100 and 10000");
            }

            if (parameters.WorkRate < 0.25m || parameters.WorkRate > 1.5m)
            {
                throw new ArgumentException($"Invalid value, the {nameof(parameters.WorkRate)} value must be between 0.25 and 1.5");
            }

            if (parameters.Gender != 'M' && parameters.Gender != 'F')
            {
                throw new ArgumentException($"Invalid value, the value of the {nameof(parameters.Gender)} variable must be M or F");
            }
        }

        /// <summary>
        /// Validate string.
        /// </summary>
        /// <param name="inputString">String for validation.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> StringValidator(string inputString)
        {
            string message = string.Empty;
            bool flag = true;
            if (inputString == null)
            {
                message = "parameter must not be null";
                flag = false;
            }

            if (inputString.Length < 2 || inputString.Length > 60)
            {
                message = "length must be between 2 and 60 characters";
                flag = false;
            }

            return new Tuple<bool, string>(flag, message);
        }

        /// <summary>
        /// Validate date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">Date of birthday.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            string message = string.Empty;
            bool flag = true;

            if (dateOfBirth.Year < 1950 || (dateOfBirth > currentDate))
            {
                message = "minimum date of birth 01-Jan-1950 maximum current date";
                flag = false;
            }

            return new Tuple<bool, string>(flag, message);
        }

        /// <summary>
        /// Validate salary.
        /// </summary>
        /// <param name="salary">Salary.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> SalaryValidator(short salary)
        {
            string message = string.Empty;
            bool flag = true;

            if (salary < 100 || salary > 10000)
            {
                message = "salary must be between 100 and 10000";
                flag = false;
            }

            return new Tuple<bool, string>(flag, message);
        }

        /// <summary>
        /// Validate work rate.
        /// </summary>
        /// <param name="workRate">Work Rate.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> WorkRateValidator(decimal workRate)
        {
            string message = string.Empty;
            bool flag = true;

            if (workRate < 0.25m || workRate > 1.5m)
            {
                message = "work rate must be between 0.25 and 1.5";
                flag = false;
            }

            return new Tuple<bool, string>(flag, message);
        }

        /// <summary>
        /// Validate gender.
        /// </summary>
        /// <param name="gender">Gender.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> GenderValidator(char gender)
        {
            string message = string.Empty;
            bool flag = true;

            if (gender != 'M' && gender != 'F')
            {
                message = "must be M or F";
                flag = false;
            }

            return new Tuple<bool, string>(flag, message);
        }
    }
}
