using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Printer
{
    /// <summary>Default printer.</summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IRecordPrinter" />
    public class DefaultRecordPrinter : IRecordPrinter
    {
        /// <summary>Prints the specified records.</summary>
        /// <param name="records">The records.</param>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            int count = records.Count();

            if (records is null)
            {
                Console.WriteLine("The record is not found");
            }
            else if (count < 1)
            {
                Console.WriteLine("The record is not found");
            }
            else
            {
                foreach (FileCabinetRecord record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, " +
                        $"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                        $"{record.Salary}, {record.WorkRate}, {record.Gender}");
                }
            }
        }
    }
}
