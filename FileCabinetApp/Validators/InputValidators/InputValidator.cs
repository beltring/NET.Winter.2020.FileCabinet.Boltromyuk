using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.InputValidators
{
    /// <summary>The input validator class.</summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IInputValidator"/>
    public class InputValidator : IInputValidator
    {
        /// <summary>
        /// Gets or sets the first length of the name minimum.
        /// </summary>
        /// <value>
        /// The first length of the name minimum.
        /// </value>
        public int FirstNameMinLength { get; set; }

        /// <summary>
        /// Gets or sets the first length of the name maximum.
        /// </summary>
        /// <value>
        /// The first length of the name maximum.
        /// </value>
        public int FirstNameMaxLength { get; set; }

        /// <summary>
        /// Gets or sets the last length of the name minimum.
        /// </summary>
        /// <value>
        /// The last length of the name minimum.
        /// </value>
        public int LastNameMinLength { get; set; }

        /// <summary>
        /// Gets or sets the last length of the name maximum.
        /// </summary>
        /// <value>
        /// The last length of the name maximum.
        /// </value>
        public int LastNameMaxLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum date.
        /// </summary>
        /// <value>
        /// The minimum date.
        /// </value>
        public DateTime MinDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum date.
        /// </summary>
        /// <value>
        /// The maximum date.
        /// </value>
        public DateTime MaxDate { get; set; }

        /// <summary>
        /// Gets or sets the minimum salary.
        /// </summary>
        /// <value>
        /// The minimum salary.
        /// </value>
        public short MinSalary { get; set; }

        /// <summary>
        /// Gets or sets the maximum salary.
        /// </summary>
        /// <value>
        /// The maximum salary.
        /// </value>
        public short MaxSalary { get; set; }

        /// <summary>
        /// Gets or sets the minimum office.
        /// </summary>
        /// <value>
        /// The minimum office.
        /// </value>
        public decimal MinWorkRate { get; set; }

        /// <summary>
        /// Gets or sets the maximum office.
        /// </summary>
        /// <value>
        /// The maximum office.
        /// </value>
        public decimal MaxWorkRate { get; set; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public List<char> Gender { get; internal set; }

        /// <summary>Firsts the name validator.</summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Is parameter valid.</returns>
        public Tuple<bool, string> FirstNameValidator(string firstName)
        {
            bool flag = (firstName != null)
                && (firstName.Length > this.FirstNameMinLength)
                && (firstName.Length <= this.FirstNameMaxLength)
                && !string.IsNullOrWhiteSpace(firstName);

            string message = $"Value shouldn't be null or contain only spaces and have minimum length {this.FirstNameMinLength} " +
                $"characters, maximum length {this.FirstNameMaxLength} characters";

            return new Tuple<bool, string>(flag, message);
        }

        /// <summary>Lasts the name validator.</summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Is parameter valid.</returns>
        public Tuple<bool, string> LastNameValidator(string lastName)
        {
            bool flag = (lastName != null)
                && (lastName.Length > this.LastNameMinLength)
                && (lastName.Length <= this.LastNameMaxLength)
                && !string.IsNullOrWhiteSpace(lastName);

            string message = $"Value shouldn't be null or contain only spaces and have minimum length {this.LastNameMinLength} " +
                $"characters, maximum length {this.LastNameMaxLength} characters";

            return new Tuple<bool, string>(flag, message);
        }

        /// <summary>
        /// Validate date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">Date of birthday.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            bool flag = (dateOfBirth <= this.MaxDate) && (dateOfBirth >= this.MinDate);

            string message = $"The minimum date should be {this.MinDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                $"the maximum date is {this.MaxDate.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}";

            return new Tuple<bool, string>(flag, message);
        }

        /// <summary>
        /// Validate salary.
        /// </summary>
        /// <param name="salary">Salary.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> SalaryValidator(short salary)
        {
            bool flag = salary >= this.MinSalary
                && salary <= this.MaxSalary;

            return new Tuple<bool, string>(flag, $"Value should be between {this.MinSalary} and {this.MaxSalary}");
        }

        /// <summary>
        /// Validate work rate.
        /// </summary>
        /// <param name="workRate">Work Rate.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> WorkRateValidator(decimal workRate)
        {
            bool flag = workRate >= this.MinWorkRate
                && workRate < this.MaxWorkRate;
            return new Tuple<bool, string>(flag, $"Value should be between {this.MinWorkRate} and {this.MaxWorkRate}");
        }

        /// <summary>
        /// Validate gender.
        /// </summary>
        /// <param name="gender">Gender.</param>
        /// <returns>The validation result and message are returned.</returns>
        public Tuple<bool, string> GenderValidator(char gender)
        {
            bool flag = false;
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.Gender)
            {
                sb.Append($"{item} ");
                if (item == gender)
                {
                    flag = true;
                }
            }

            return new Tuple<bool, string>(flag, $"Gender can be only {sb.ToString().Trim()}");
        }
    }
}
