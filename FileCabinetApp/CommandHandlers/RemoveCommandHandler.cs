using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Remove command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class RemoveCommandHandler : CommandHandlerBase
    {
        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "remove")
            {
                Remove(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out int id))
            {
                Console.WriteLine("The command was entered incorrectly. Input format: \"remove (int number)\".");
                return;
            }

            if (id == 0 || Program.FileCabinetService.GetStat(out int deletedRecordsCount) == 0)
            {
                Console.WriteLine($"Record #{parameters} doesn't exists.");
                return;
            }

            try
            {
                Program.FileCabinetService.Remove(id);
                Console.WriteLine($"Record #{parameters} is removed.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
