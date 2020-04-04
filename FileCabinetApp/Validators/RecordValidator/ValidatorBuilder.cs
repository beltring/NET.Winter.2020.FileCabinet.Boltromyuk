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
        public ValidatorBuilder()
        {
            this.validators = new List<IRecordValidator>();
        }

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
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
        {
            this.validators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        /// <summary>Validates the last name.</summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
        {
            this.validators.Add(new LastNameValidator(minLength, maxLength));
            return this;
        }

        /// <summary>Validates the date of birth.</summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>Validates the salary.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateSalary(short min, short max)
        {
            this.validators.Add(new SalaryValidator(min, max));
            return this;
        }

        /// <summary>Validates the work rate.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateWorkRate(decimal min, decimal max)
        {
            this.validators.Add(new WorkRateValidator(min, max));
            return this;
        }

        /// <summary>Validates the gender.</summary>
        /// <param name="gender">The gender.</param>
        /// <returns>Validator builder.</returns>
        public ValidatorBuilder ValidateGender(char[] gender)
        {
            this.validators.Add(new GenderValidator(gender));
            return this;
        }
    }
}
