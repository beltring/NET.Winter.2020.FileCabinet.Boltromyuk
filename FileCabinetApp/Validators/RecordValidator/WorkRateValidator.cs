using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>Work rate validator.</summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IRecordValidator" />
    internal class WorkRateValidator : IRecordValidator
    {
        private readonly decimal minWorkRate;
        private readonly decimal maxWorkRate;

        /// <summary>Initializes a new instance of the <see cref="WorkRateValidator"/> class.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        internal WorkRateValidator(decimal min, decimal max)
        {
            this.minWorkRate = min;
            this.maxWorkRate = max;
        }

        /// <summary>Validate parameters.</summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            if (parameters.WorkRate < this.minWorkRate || parameters.WorkRate > this.maxWorkRate)
            {
                throw new ArgumentException($"Invalid value, the {nameof(parameters.WorkRate)} value must be between 0.25 and 1.5");
            }
        }
    }
}
