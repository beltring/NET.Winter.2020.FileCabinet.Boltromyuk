using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Exit command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class ExitCommandHandler : CommandHandlerBase
    {
        private static FileStream stream;
        private readonly Action<bool> isRunningAction;

        /// <summary>Initializes a new instance of the <see cref="ExitCommandHandler"/> class.</summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="isRunningAction">is running.</param>
        public ExitCommandHandler(FileStream fileStream, Action<bool> isRunningAction)
        {
            stream = fileStream;
            this.isRunningAction = isRunningAction;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "exit")
            {
                this.Exit(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            this.isRunningAction(false);
            stream?.Close();
        }
    }
}
