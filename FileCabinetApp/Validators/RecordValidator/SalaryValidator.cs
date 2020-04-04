using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>Salary validator.</summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IRecordValidator" />
    internal class SalaryValidator : IRecordValidator
    {
        private readonly short minSalary;
        private readonly short maxSalary;

        /// <summary>Initializes a new instance of the <see cref="SalaryValidator"/> class.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        internal SalaryValidator(short min, short max)
        {
            this.minSalary = min;
            this.maxSalary = max;
        }

        /// <summary>Validate parameters.</summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            if (parameters.Salary < this.minSalary || parameters.Salary > this.maxSalary)
            {
                throw new ArgumentException($"Invalid value, the {nameof(parameters.Salary)} value must be " +
                    $"between {this.minSalary} and {this.maxSalary}");
            }
        }
    }
}
