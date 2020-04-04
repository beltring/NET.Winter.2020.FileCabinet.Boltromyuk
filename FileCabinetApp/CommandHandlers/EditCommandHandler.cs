using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Edit command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class EditCommandHandler : CommandHandlerBase
    {
        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "edit")
            {
                Edit(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void Edit(string parameters)
        {
            int id = 0;

            id = int.Parse(parameters, CultureInfo.InvariantCulture);
            try
            {
                if (Program.FileCabinetService.CheckId(id, out int index))
                {
                    RecordArgs eventArgs = ConsoleReader.ConsoleRead();

                    Program.FileCabinetService.EditRecord(id, eventArgs);
                    Console.WriteLine($"Record #{id} is updated.");
                }
                else
                {
                    Console.WriteLine("There is no record with this id");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
