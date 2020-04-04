using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Purge command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class PurgeCommandHandler : CommandHandlerBase
    {
        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "purge")
            {
                Purge(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void Purge(string parameters)
        {
            Program.FileCabinetService.Purge(out int deletedRecordsCount, out int recordsCount);
            if (Program.FileCabinetService is FileCabinetFilesystemService)
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
