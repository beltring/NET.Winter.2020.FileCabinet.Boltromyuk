using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp
{
    /// <summary>
    /// This class provides data validation.
    /// </summary>
    internal class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Custom validate parameters.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordEventArgs parameters)
        {
            DateTime date = new DateTime(2010, 12, 31);

            if (parameters == null)
            {
                throw new ArgumentException($"{nameof(parameters)} can't be null.");
            }

            if (parameters.FirstName == null)
            {
                throw new ArgumentNullException($"The {nameof(parameters.FirstName)} parameter must not be null");
            }

            if (parameters.LastName == null)
            {
                throw new ArgumentNullException($"The {nameof(parameters.LastName)} parameter must not be null");
            }

            if (parameters.FirstName.Length < 3 || parameters.FirstName.Length > 20)
            {
                throw new ArgumentException($"The length of {nameof(parameters.FirstName)} must be between 3 and 20 characters");
            }

            if (parameters.LastName.Length < 3 || parameters.LastName.Length > 20)
            {
                throw new ArgumentException($"The length of {nameof(parameters.LastName)} must be between 3 and 20 characters");
            }

            if (parameters.DateOfBirth.Year < 1960 || (parameters.DateOfBirth > date))
            {
                throw new ArgumentException("Minimum date of birth 01-Jan-1960 maximum 31-Dec-2010");
            }

            if (parameters.Salary < 300 || parameters.Salary > 6500)
            {
                throw new ArgumentException($"Invalid value, the {nameof(parameters.Salary)} value must be between 300 and 6500");
            }

            if (parameters.WorkRate < 0.25m || parameters.WorkRate > 1.0m)
            {
                throw new ArgumentException($"Invalid value, the {nameof(parameters.WorkRate)} value must be between 0.25 and 1.0");
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

            if (inputString.Length < 3 || inputString.Length > 20)
            {
                message = "length must be between 3 and 20 characters";
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
            DateTime date = new DateTime(2010, 12, 31);

            string message = string.Empty;
            bool flag = true;

            if (dateOfBirth.Year < 1960 || (dateOfBirth > date))
            {
                message = "Minimum date of birth 01-Jan-1960 maximum 31-Dec-2010";
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

            if (salary < 300 || salary > 6500)
            {
                message = "salary must be between 300 and 6500";
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

            if (workRate < 0.25m || workRate > 1.0m)
            {
                message = "work rate must be between 0.25 and 1.0";
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
