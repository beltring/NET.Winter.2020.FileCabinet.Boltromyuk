using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Edit command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class EditCommandHandler : ServiceCommandHandlerBase
    {
        private IInputValidator validator;

        /// <summary>Initializes a new instance of the <see cref="EditCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        /// <param name="validator">The validator.</param>
        public EditCommandHandler(IFileCabinetService service, IInputValidator validator)
            : base(service)
        {
            this.validator = validator;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "edit")
            {
                this.Edit(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private void Edit(string parameters)
        {
            try
            {
                int id = int.Parse(parameters, CultureInfo.InvariantCulture);
                if (this.Service.CheckId(id, out int index))
                {
                    RecordArgs eventArgs = ConsoleReader.ConsoleRead(this.validator);

                    this.Service.EditRecord(id, eventArgs);
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
            catch (FormatException)
            {
                Console.WriteLine("The command was entered incorrectly. Input format: \"edit Id(integer number)\".");
            }
        }
    }
}
