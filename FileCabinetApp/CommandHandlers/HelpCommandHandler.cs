using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Help command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "displays statistics for the entries", "The 'stat' command displays statistics for the entries." },
            new string[] { "create", "creates a record", "The 'create' command creates a record." },
            new string[] { "list", "returns a list of records added to the service", "The 'list' command returns a list of records added to the service" },
            new string[] { "edit", "editing record", "The 'edit' command editing record" },
            new string[] { "find", "find records", "The 'find' command find records" },
            new string[] { "export", "export records", "The 'export' command export records to csv or xml file." },
            new string[] { "import", "import records", "The 'import' command import records to csv or xml file." },
            new string[] { "remove", "removes a record by id", "The 'remove' command remove record" },
            new string[] { "purge", "defragment a data file", "The 'purge' command defragment a data file." },
        };

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "help")
            {
                PrintHelp(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
