using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Import command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class ImportCommandHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        /// <summary>Initializes a new instance of the <see cref="ImportCommandHandler"/> class.</summary>
        /// <param name="service">The service.</param>
        public ImportCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "import")
            {
                this.Import(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private void Import(string parameters)
        {
            string[] commands = parameters.Split(' ');

            if (commands.Length > 1)
            {
                string fileFormat = commands[0];
                string path = commands[1];

                if (!File.Exists(path))
                {
                    Console.WriteLine($"Import error: file {nameof(path)} is not exist.");
                    return;
                }

                switch (fileFormat)
                {
                    case "csv":
                        this.ImportFromCsv(path);
                        break;
                    case "xml":
                        this.ImportFromXML(path);
                        break;
                    default:
                        Console.WriteLine($"Unknown file format '{fileFormat}'.Available formats: 'csv', 'xml'.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("The command was entered incorrectly. Input format: \"import type path\".");
            }
        }

        private void ImportFromCsv(string path)
        {
            FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot();
            using StreamReader reader = new StreamReader(path);

            snapshot.LoadFromCSV(reader, out int countRecords);
            this.service.Restore(snapshot, out Dictionary<int, string> exceptions);

            countRecords -= exceptions.Count;

            foreach (var ex in exceptions)
            {
                Console.WriteLine($"Record #{ex.Key} was not imported.Error:{ex.Value}");
            }

            if (countRecords > 0)
            {
                Console.WriteLine($"{countRecords} records were imported from {path}.");
            }
            else
            {
                Console.WriteLine("Records were not imported, and the file is empty.");
            }
        }

        private void ImportFromXML(string path)
        {
            FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot();

            using StreamReader reader = new StreamReader(path);
            snapshot.LoadFromXML(reader, out int countRecords);
            this.service.Restore(snapshot, out Dictionary<int, string> exceptions);

            countRecords -= exceptions.Count;

            foreach (var ex in exceptions)
            {
                Console.WriteLine($"Record #{ex.Key} was not imported.Error:{ex.Value}");
            }

            if (countRecords > 0)
            {
                Console.WriteLine($"{countRecords} records were imported from {path}.");
            }
            else
            {
                Console.WriteLine("Records were not imported, and the file is empty.");
            }
        }
    }
}
