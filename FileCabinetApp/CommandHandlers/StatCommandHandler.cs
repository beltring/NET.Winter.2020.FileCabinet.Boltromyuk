using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Stat command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class StatCommandHandler : CommandHandlerBase
    {
        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "stat")
            {
                Stat(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.FileCabinetService.GetStat(out int deletedRecordsCount);
            Console.WriteLine($"{recordsCount} record(s). Number of deleted records: {deletedRecordsCount}.");
        }
    }
}
