using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>Last Name Validator.</summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IRecordValidator" />
    internal class LastNameValidator : IRecordValidator
    {
        private const string NamePattern = @"^[a-zA-Z '.-]*$";
        private readonly int minLength;
        private readonly int maxLength;

        /// <summary>Initializes a new instance of the <see cref="LastNameValidator"/> class.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        internal LastNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        /// <summary>Validate parameters.</summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            var lastname = parameters.LastName;
            if (lastname is null)
            {
                throw new ArgumentNullException($"{parameters.LastName} cannot be null.");
            }

            if ((lastname.Length < this.minLength)
                && (lastname.Length >= this.maxLength)
                && !Regex.IsMatch(lastname, NamePattern)
                && string.IsNullOrWhiteSpace(lastname))
            {
                throw new ArgumentException($"The length of {nameof(parameters.FirstName)} must be between " +
                    $"{this.minLength} and {this.maxLength} characters");
            }
        }
    }
}
