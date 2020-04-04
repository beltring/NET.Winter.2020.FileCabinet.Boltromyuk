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
    internal class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Custom validate parameters.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            DateTime date = new DateTime(2010, 12, 31);

            if (parameters == null)
            {
                throw new ArgumentException($"{nameof(parameters)} can't be null.");
            }

            new FirstNameValidator(3, 20).ValidateParameters(parameters);
            new LastNameValidator(3, 20).ValidateParameters(parameters);
            new DateOfBirthValidator(new DateTime(1970, 1, 1), date).ValidateParameters(parameters);
            new SalaryValidator(300, 6500).ValidateParameters(parameters);
            new WorkRateValidator(0.25m, 1.0m).ValidateParameters(parameters);
            new GenderValidator(new char[] { 'M', 'F' }).ValidateParameters(parameters);
        }
    }
}
