using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>Composite validator.</summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IRecordValidator" />
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">The validators.</param>
        protected CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = validators.ToList();
        }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public void ValidateParameters(RecordArgs parameters)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(parameters);
            }
        }
    }
}
