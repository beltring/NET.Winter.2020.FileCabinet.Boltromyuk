using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Validators.RecordValidator;

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

            new FirstNameValidator(2, 30).ValidateParameters(parameters);
            new LastNameValidator(2, 30).ValidateParameters(parameters);
            new DateOfBirthValidator(new DateTime(1950, 1, 1), currentDate).ValidateParameters(parameters);
            new SalaryValidator(100, 10000).ValidateParameters(parameters);
            new WorkRateValidator(0.25m, 1.5m).ValidateParameters(parameters);
            new GenderValidator(new char[] { 'M', 'F' }).ValidateParameters(parameters);
        }
    }
}
