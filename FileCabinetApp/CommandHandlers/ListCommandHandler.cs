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
        private readonly Action<IEnumerable<FileCabinetRecord>> printer;

        /// <summary>Initializes a new instance of the <see cref="ListCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        /// <param name="printer">The printer.</param>
        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer)
            : base(service)
        {
            this.printer = printer;
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

        private void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> records = this.Service.GetRecords();

            var sortRecords = records.OrderBy(x => x.Id).ToList();

            this.printer(sortRecords);
        }
    }
}
