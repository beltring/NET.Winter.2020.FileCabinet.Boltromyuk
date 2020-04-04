using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>Builder extension.</summary>
    public static class ValidatorBuilderExtensions
    {
        /// <summary>Creates the default.</summary>
        /// <param name="validatorBuilder">The validator builder.</param>
        /// <returns>Record validator.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder validatorBuilder)
        {
            var validator = new ValidatorBuilder()
                .ValidateFirstName(2, 60)
                .ValidateLastName(2, 60)
                .ValidateDateOfBirth(new DateTime(1950, 1, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                .ValidateSalary(100, 10000)
                .ValidateWorkRate(0.25m, 1.5m)
                .ValidateGender(new char[] { 'M', 'F' })
                .Create();

            return validator;
        }

        /// <summary>Creates the custom.</summary>
        /// <param name="validatorBuilder">The validator builder.</param>
        /// <returns>Record validator.</returns>
        public static IRecordValidator CreateCustom(this ValidatorBuilder validatorBuilder)
        {
            var validator = new ValidatorBuilder()
                .ValidateFirstName(3, 20)
                .ValidateLastName(3, 20)
                .ValidateDateOfBirth(new DateTime(1970, 1, 1), new DateTime(2010, 12, 31))
                .ValidateSalary(300, 6500)
                .ValidateWorkRate(0.25m, 1.0m)
                .ValidateGender(new char[] { 'M', 'F' })
                .Create();

            return validator;
        }
    }
}
