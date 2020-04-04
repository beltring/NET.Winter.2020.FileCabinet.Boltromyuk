using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.InputValidators
{
    /// <summary>Input validator builder.</summary>
    public static class InputValidatorBuilder
    {
        private static InputValidator validator;

        /// <summary>
        /// Creates the default.
        /// </summary>
        /// <returns>Default input validator.</returns>
        public static IInputValidator CreateDefault() => CreateValidator("default");

        /// <summary>
        /// Creates the custom.
        /// </summary>
        /// <returns>Custom input validator.</returns>
        public static IInputValidator CreateCustom() => CreateValidator("custom");

        private static IInputValidator CreateValidator(string validatorType)
        {
            validator = new InputValidator();
            if (validatorType.Equals("custom", StringComparison.InvariantCultureIgnoreCase))
            {
                validator.FirstNameMinLength = 3;
                validator.FirstNameMaxLength = 20;
                validator.LastNameMinLength = 3;
                validator.LastNameMaxLength = 20;
                validator.MinDate = new DateTime(1970, 1, 1);
                validator.MaxDate = new DateTime(2010, 12, 31);
                validator.MinSalary = 300;
                validator.MaxSalary = 6500;
                validator.MinWorkRate = 0.25m;
                validator.MaxWorkRate = 1.0m;
                validator.Gender = new List<char> { 'M', 'F' };
            }
            else
            {
                validator.FirstNameMinLength = 2;
                validator.FirstNameMaxLength = 30;
                validator.LastNameMinLength = 2;
                validator.LastNameMaxLength = 30;
                validator.MinDate = new DateTime(1950, 1, 1);
                validator.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                validator.MinSalary = 100;
                validator.MaxSalary = 10000;
                validator.MinWorkRate = 0.25m;
                validator.MaxWorkRate = 1.5m;
                validator.Gender = new List<char> { 'M', 'F' };
            }

            return validator;
        }
    }
}
