using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>Class FileCabinetRecordCsvWriter.</summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.</summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="ArgumentNullException">Throw when writer is null.</exception>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} can't be null.");
            }

            this.writer = writer;
            writer.WriteLine("Id,First Name,Last Name,Date of Birth,Salary,Work rate,Gender");
        }

        /// <summary>Writes the specified record.</summary>
        /// <param name="record">The record.</param>
        /// <exception cref="ArgumentNullException">Throw when record is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{nameof(record)} can't be null");
            }

            string result = $"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}," +
                    $"{record.Salary},{record.WorkRate},{record.Gender}";

            this.writer.WriteLine(result);
        }
    }
}
