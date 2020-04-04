using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    ///   <para>Date of birth validator.</para>
    /// </summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IRecordValidator" />
    internal class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime to;

        /// <summary>Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.</summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        internal DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>Validate parameters.</summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            if (parameters.DateOfBirth == null)
            {
                throw new ArgumentNullException($"{parameters.DateOfBirth} cannot be null.", nameof(parameters.DateOfBirth));
            }

            if (parameters.DateOfBirth > this.to || parameters.DateOfBirth < this.from)
            {
                throw new ArgumentException(
                    $"The {nameof(parameters.DateOfBirth)} can't be less than {this.from.ToShortDateString()} and larger than {this.to.ToShortDateString()}.");
            }
        }
    }
}
