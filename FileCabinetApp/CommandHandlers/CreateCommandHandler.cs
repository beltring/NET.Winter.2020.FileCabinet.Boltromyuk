using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Create command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private IInputValidator validator;

        /// <summary>Initializes a new instance of the <see cref="CreateCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        /// <param name="validator">The validator.</param>
        public CreateCommandHandler(IFileCabinetService service, IInputValidator validator)
            : base(service)
        {
            this.validator = validator;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "create")
            {
                this.Create(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private void Create(string parameters)
        {
            RecordArgs eventArgs = ConsoleReader.ConsoleRead(this.validator);

            int id = this.Service.CreateRecord(eventArgs);
            Console.WriteLine($"Record #{id} is created.");
        }
    }
}
