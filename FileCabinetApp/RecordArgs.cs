using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class describes a group of parameters.
    /// </summary>
    public sealed class RecordArgs
    {
        /// <summary>
        /// Gets or sets firstName.
        /// </summary>
        /// <value>
        /// FirstName.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets lastName.
        /// </summary>
        /// <value>
        /// LastName.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets dateOfBirth.
        /// </summary>
        /// <value>
        /// DateOfBirth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets salary.
        /// </summary>
        /// <value>
        /// Salary.
        /// </value>
        public short Salary { get; set; }

        /// <summary>
        /// Gets or sets workRate.
        /// </summary>
        /// <value>
        /// WorkRate.
        /// </value>
        public decimal WorkRate { get; set; }

        /// <summary>
        /// Gets or sets gender.
        /// </summary>
        /// <value>
        /// Gender.
        /// </value>
        public char Gender { get; set; }
    }
}
