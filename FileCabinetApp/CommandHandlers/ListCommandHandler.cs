using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>List command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class ListCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="ListCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        public ListCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "list")
            {
                this.List(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void Print(ICollection<FileCabinetRecord> records)
        {
            if (records is null)
            {
                Console.WriteLine("The record is not found");
            }
            else if (records.Count < 1)
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

        private void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> records = this.Service.GetRecords();

            var sortRecords = records.OrderBy(x => x.Id).ToList();

            Print(sortRecords);
        }
    }
}
