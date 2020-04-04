using System;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Validators.RecordValidator;

namespace FileCabinetApp
{
    /// <summary>
    /// This class provides data validation.
    /// </summary>
    internal class CustomValidator : CompositeValidator
    {
        /// <summary>Initializes a new instance of the <see cref="CustomValidator"/> class.</summary>
        public CustomValidator()
            : base(new IRecordValidator[]
            {
            new FirstNameValidator(3, 20),
            new LastNameValidator(3, 20),
            new DateOfBirthValidator(new DateTime(1970, 1, 1), new DateTime(2010, 12, 31)),
            new SalaryValidator(300, 6500),
            new WorkRateValidator(0.25m, 1.0m),
            new GenderValidator(new char[] { 'M', 'F' }),
            })
        {
        }
    }
}
