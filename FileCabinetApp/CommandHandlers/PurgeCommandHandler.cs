using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Purge command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class PurgeCommandHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        /// <summary>Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        public PurgeCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "purge")
            {
                this.Purge(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private void Purge(string parameters)
        {
            this.service.Purge(out int deletedRecordsCount, out int recordsCount);

            if (this.service is FileCabinetFilesystemService)
            {
                Console.WriteLine($"Data file processing is completed: {deletedRecordsCount} of {recordsCount} records were purged.");
            }
            else
            {
                Console.WriteLine("This command is not supported by this mode of operation.");
            }
        }
    }
}
