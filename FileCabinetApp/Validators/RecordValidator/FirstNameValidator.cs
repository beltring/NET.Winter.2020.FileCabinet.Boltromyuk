using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>First Name Validator.</summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IRecordValidator" />
    internal class FirstNameValidator : IRecordValidator
    {
        private const string NamePattern = @"^[a-zA-Z '.-]*$";
        private readonly int minLength;
        private readonly int maxLength;

        /// <summary>Initializes a new instance of the <see cref="FirstNameValidator"/> class.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        internal FirstNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        /// <summary>Validate parameters.</summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            var firstname = parameters.FirstName;
            if (firstname is null)
            {
                throw new ArgumentNullException($"{nameof(parameters.FirstName)} cannot be null.");
            }

            if ((firstname.Length < this.minLength)
                && (firstname.Length >= this.maxLength)
                && !Regex.IsMatch(firstname, NamePattern)
                && string.IsNullOrWhiteSpace(firstname))
            {
                throw new ArgumentException($"The length of {nameof(parameters.FirstName)} must be between " +
                    $"{this.minLength} and {this.maxLength} characters");
            }
        }
    }
}
