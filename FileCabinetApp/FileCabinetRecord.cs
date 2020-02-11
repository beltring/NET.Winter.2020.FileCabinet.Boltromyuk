using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class describes the record entity.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        /// <value>
        /// Integer number.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        /// <value>
        /// FirstName.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        /// <value>
        /// LastName.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets DateOfBirth.
        /// </summary>
        /// <value>
        /// DateOfBirth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets Salary.
        /// </summary>
        /// <value>
        /// Salary.
        /// </value>
        public short Salary { get; set; }

        /// <summary>
        /// Gets or sets WorkRate.
        /// </summary>
        /// <value>
        /// WorkRate.
        /// </value>
        public decimal WorkRate { get; set; }

        /// <summary>
        /// Gets or sets Gender.
        /// </summary>
        /// <value>
        /// Gender.
        /// </value>
        public char Gender { get; set; }
    }
}
