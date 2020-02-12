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
        public void ValidateParameters(RecordsParameters parameters)
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
    }
}
