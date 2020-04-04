using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Stat command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>Initializes a new instance of the <see cref="StatCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "stat")
            {
                this.Stat(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private void Stat(string parameters)
        {
            var recordsCount = this.Service.GetStat(out int deletedRecordsCount);
            Console.WriteLine($"{recordsCount} record(s). Number of deleted records: {deletedRecordsCount}.");
        }
    }
}
