using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Remove command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class RemoveCommandHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        /// <summary>Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        public RemoveCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "remove")
            {
                this.Remove(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out int id))
            {
                Console.WriteLine("The command was entered incorrectly. Input format: \"remove (int number)\".");
                return;
            }

            if (id == 0 || this.service.GetStat(out int deletedRecordsCount) == 0)
            {
                Console.WriteLine($"Record #{parameters} doesn't exists.");
                return;
            }

            try
            {
                this.service.Remove(id);
                Console.WriteLine($"Record #{parameters} is removed.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
