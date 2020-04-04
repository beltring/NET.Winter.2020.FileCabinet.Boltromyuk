using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Validator builder.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>Initializes a new instance of the <see cref="ValidatorBuilder"/> class.</summary>
        /// <param name="validators">The validators.</param>
        public ValidatorBuilder(List<IRecordValidator> validators)
        {
            this.validators = validators;
        }

        /// <summary>Creates this instance.</summary>
        /// <returns>Validator.</returns>
        public IRecordValidator Create() => new CompositeValidator(this.validators);

        /// <summary>Validates the first name.</summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        public void ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
        }

        /// <summary>Validates the last name.</summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        public void ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new LastNameValidator(minLength, maxLength));
        }

        /// <summary>Validates the date of birth.</summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
        }

        /// <summary>Validates the salary.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public void ValidateSalary(short min, short max)
        {
            this.validators.Add(new SalaryValidator(min, max));
        }

        /// <summary>Validates the work rate.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public void ValidateWorkRate(decimal min, decimal max)
        {
            this.validators.Add(new WorkRateValidator(min, max));
        }

        /// <summary>Validates the gender.</summary>
        /// <param name="gender">The gender.</param>
        public void ValidateGender(char[] gender)
        {
            this.validators.Add(new GenderValidator(gender));
        }
    }
}
