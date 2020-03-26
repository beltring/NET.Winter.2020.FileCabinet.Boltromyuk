using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Interfaces
{
    /// <summary>
    /// Interface IRecordValidator.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validate parameters.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        void ValidateParameters(RecordArgs parameters);

        /// <summary>
        /// Validate string.
        /// </summary>
        /// <param name="inputString">String for validation.</param>
        /// <returns>The validation result and message are returned.</returns>
        Tuple<bool, string> StringValidator(string inputString);

        /// <summary>
        /// Validate date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">Date of birthday.</param>
        /// <returns>The validation result and message are returned.</returns>
        Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth);

        /// <summary>
        /// Validate salary.
        /// </summary>
        /// <param name="salary">Salary.</param>
        /// <returns>The validation result and message are returned.</returns>
        Tuple<bool, string> SalaryValidator(short salary);

        /// <summary>
        /// Validate work rate.
        /// </summary>
        /// <param name="workRate">Work Rate.</param>
        /// <returns>The validation result and message are returned.</returns>
        Tuple<bool, string> WorkRateValidator(decimal workRate);

        /// <summary>
        /// Validate gender.
        /// </summary>
        /// <param name="gender">Gender.</param>
        /// <returns>The validation result and message are returned.</returns>
        Tuple<bool, string> GenderValidator(char gender);
    }
}
