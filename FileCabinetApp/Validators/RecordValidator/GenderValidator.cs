using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    ///   <para>Gender validator.</para>
    /// </summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IRecordValidator" />
    internal class GenderValidator : IRecordValidator
    {
        private readonly char[] gender;

        /// <summary>Initializes a new instance of the <see cref="GenderValidator"/> class.</summary>
        /// <param name="gender">The gender.</param>
        internal GenderValidator(char[] gender)
        {
            if (gender == null)
            {
                throw new ArgumentNullException($"{nameof(gender)} cannot be null.");
            }

            if (gender.Length == 0)
            {
                throw new ArgumentException($"{nameof(gender)} cannot be empty.");
            }

            this.gender = gender;
        }

        /// <summary>Validate parameters.</summary>
        /// <param name="parameters">Parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            bool flag = true;
            foreach (var item in this.gender)
            {
                if (item == parameters.Gender)
                {
                    flag = false;
                }
            }

            if (flag)
            {
                string exc = null;
                foreach (var item in this.gender)
                {
                    exc += item + ",";
                }

                throw new ArgumentException($"Invalid value, the value of the {nameof(this.gender)} can be only {exc}.");
            }
        }
    }
}
