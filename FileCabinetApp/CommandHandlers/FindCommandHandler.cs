using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Find command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class FindCommandHandler : CommandHandlerBase
    {
        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "find")
            {
                Find(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void Find(string parameters)
        {
            string[] param = parameters.Split(' ');
            ReadOnlyCollection<FileCabinetRecord> records = null;

            if (param.Length > 1)
            {
                string search = param[1].Trim('"');
                string searchParam = param[0].ToLower(CultureInfo.CurrentCulture);

                switch (searchParam)
                {
                    case "firstname":
                        records = Program.FileCabinetService.FindByFirstName(search);
                        Print(records);
                        break;
                    case "lastname":
                        records = Program.FileCabinetService.FindByLastName(search);
                        Print(records);
                        break;
                    case "dateofbirth":
                        DateTime dateofbirth = DateTime.ParseExact(search, "yyyy-MMM-dd", CultureInfo.InvariantCulture);
                        records = Program.FileCabinetService.FindByDateOfBirth(dateofbirth);
                        Print(records);
                        break;
                    default:
                        Console.WriteLine("There is no such category.Available categories:'firstname', 'lastname', 'dateofbirth'");
                        break;
                }
            }
            else
            {
                Console.WriteLine("The command was entered incorrectly. Input format: \"find category item\".");
            }
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
    }
}
