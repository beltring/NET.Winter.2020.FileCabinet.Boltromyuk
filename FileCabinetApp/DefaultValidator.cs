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
        public void ValidateParameters(RecordsParameters parameters)
        {
            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

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
    }
}
