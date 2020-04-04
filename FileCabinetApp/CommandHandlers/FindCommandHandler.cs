using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Find command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter printer;

        /// <summary>Initializes a new instance of the <see cref="FindCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        /// <param name="printer">The printer.</param>
        public FindCommandHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "find")
            {
                this.Find(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private void Find(string parameters)
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
                        records = this.Service.FindByFirstName(search);
                        this.printer.Print(records);
                        break;
                    case "lastname":
                        records = this.Service.FindByLastName(search);
                        this.printer.Print(records);
                        break;
                    case "dateofbirth":
                        DateTime dateofbirth = DateTime.ParseExact(search, "yyyy-MMM-dd", CultureInfo.InvariantCulture);
                        records = this.Service.FindByDateOfBirth(dateofbirth);
                        this.printer.Print(records);
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
    }
}
