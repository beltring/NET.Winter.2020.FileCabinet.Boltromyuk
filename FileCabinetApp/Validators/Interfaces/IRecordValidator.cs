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
    }
}
