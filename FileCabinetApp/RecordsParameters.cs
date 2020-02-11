using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class describes a group of parameters.
    /// </summary>
    public sealed class RecordsParameters
    {
        /// <summary>
        /// Gets firstName.
        /// </summary>
        /// <value>
        /// FirstName.
        /// </value>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets lastName.
        /// </summary>
        /// <value>
        /// LastName.
        /// </value>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets dateOfBirth.
        /// </summary>
        /// <value>
        /// DateOfBirth.
        /// </value>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Gets salary.
        /// </summary>
        /// <value>
        /// Salary.
        /// </value>
        public short Salary { get; private set; }

        /// <summary>
        /// Gets workRate.
        /// </summary>
        /// <value>
        /// WorkRate.
        /// </value>
        public decimal WorkRate { get; private set; }

        /// <summary>
        /// Gets gender.
        /// </summary>
        /// <value>
        /// Gender.
        /// </value>
        public char Gender { get; private set; }

        /// <summary>
        /// This method reads parameter input from the console.
        /// </summary>
        public void ReadParameters()
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            cultureInfo.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";

            Console.Write("First name:");
            this.FirstName = Console.ReadLine();
            Console.Write("Last name:");
            this.LastName = Console.ReadLine();
            Console.Write("Date of birth:");
            string date = Console.ReadLine();
            this.DateOfBirth = DateTime.ParseExact(date, "d", cultureInfo);
            Console.Write("Salary:");
            this.Salary = short.Parse(Console.ReadLine(), cultureInfo);
            Console.Write("Work rate:");
            this.WorkRate = decimal.Parse(Console.ReadLine(), cultureInfo);
            Console.Write("Gender(M/F):");
            this.Gender = char.Parse(Console.ReadLine());
        }
    }
}
