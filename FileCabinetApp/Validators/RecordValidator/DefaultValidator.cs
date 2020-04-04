using System;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Validators.RecordValidator;

namespace FileCabinetApp
{
    /// <summary>
    /// This class provides data validation.
    /// </summary>
    internal class DefaultValidator : CompositeValidator
    {
        /// <summary>Initializes a new instance of the <see cref="DefaultValidator"/> class.</summary>
        public DefaultValidator()
            : base(new IRecordValidator[]
            {
            new FirstNameValidator(2, 30),
            new LastNameValidator(2, 30),
            new DateOfBirthValidator(new DateTime(1950, 1, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)),
            new SalaryValidator(100, 10000),
            new WorkRateValidator(0.25m, 1.5m),
            new GenderValidator(new char[] { 'M', 'F' }),
            })
        {
        }
    }
}
