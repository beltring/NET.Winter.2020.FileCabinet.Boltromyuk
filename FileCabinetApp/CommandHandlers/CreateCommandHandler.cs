using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Create command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class CreateCommandHandler : CommandHandlerBase
    {
        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "create")
            {
                Create(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void Create(string parameters)
        {
            RecordArgs eventArgs = ConsoleReader.ConsoleRead();

            int id = Program.FileCabinetService.CreateRecord(eventArgs);
            Console.WriteLine($"Record #{id} is created.");
        }
    }
}
