using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Enums;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp
{
    /// <summary>
    /// Class Program.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Pavel Boltromyuk";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const string DefaultBinaryFileName = "cabinet-records.db";
        private static FileStream fileStream;

        /// <summary>Gets or sets a value indicating whether this instance is running.</summary>
        /// <value>
        ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
        public static bool IsRunning { get; set; }

        /// <summary>Gets the file cabinet service.</summary>
        /// <value>The file cabinet service.</value>
        public static IFileCabinetService FileCabinetService { get; private set; }

        /// <summary>Gets the type of the validator.</summary>
        /// <value>The type of the validator.</value>
        public static ValidatorType ValidatorType { get; private set; }

        /// <summary>Gets the type of the service.</summary>
        /// <value>The type of the service.</value>
        public static ServiceType ServiceType { get; private set; }

        /// <summary>Gets the validator.</summary>
        /// <value>The validator.</value>
        public static IRecordValidator Validator { get; private set; }

        /// <summary>
        /// The method that starts the program execution.
        /// </summary>
        /// <param name="args">Input parameters.</param>
        public static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException($"{nameof(args)} can't be null.");
            }

            IsRunning = true;
            ParseArguments(args);
            CreateService(ValidatorType, ServiceType);
            var commandHandler = CreateCommandHandlers();

            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine($"Using {ValidatorType} validation rules.");
            Console.WriteLine($"The {ServiceType} service.");
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commandHandler.Handle(new AppCommandRequest(command, parameters));
            }
            while (IsRunning);
        }

        private static void ParseArguments(string[] args)
        {
            bool isCustom = args.Any(x => x.Contains(ValidatorType.Custom.ToString(), StringComparison.OrdinalIgnoreCase));
            bool isFile = args.Any(x => x.Contains(ServiceType.File.ToString(), StringComparison.OrdinalIgnoreCase));

            ValidatorType = isCustom ? ValidatorType.Custom : ValidatorType.Default;
            ServiceType = isFile ? ServiceType.File : ServiceType.Memory;
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var exitHandler = new ExitCommandHandler();
            var helpHandler = new HelpCommandHandler();
            var createHandle = new CreateCommandHandler();
            var editHandler = new EditCommandHandler();
            var findHandler = new FindCommandHandler();
            var exportHandler = new ExportCommandHandler();
            var importHandler = new ImportCommandHandler();
            var removeHandler = new RemoveCommandHandler();
            var listHandle = new ListCommandHandler();
            var statHandler = new StatCommandHandler();
            var purgeHadler = new PurgeCommandHandler();
            var missedCommandHandler = new MissedCommandHandler();
            helpHandler
                .SetNext(exitHandler)
                .SetNext(listHandle)
                .SetNext(statHandler)
                .SetNext(createHandle)
                .SetNext(editHandler)
                .SetNext(findHandler)
                .SetNext(removeHandler)
                .SetNext(exportHandler)
                .SetNext(importHandler)
                .SetNext(purgeHadler)
                .SetNext(missedCommandHandler);
            return helpHandler;
        }

        private static void CreateService(ValidatorType validatorType, ServiceType serviceType)
        {
            switch (validatorType)
            {
                case ValidatorType.Custom:
                    Validator = new CustomValidator();
                    break;
                case ValidatorType.Default:
                    Validator = new DefaultValidator();
                    break;
            }

            switch (serviceType)
            {
                case ServiceType.File:
                    fileStream = new FileStream(DefaultBinaryFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    Program.FileCabinetService = new FileCabinetFilesystemService(Validator, fileStream);
                    break;
                case ServiceType.Memory:
                    Program.FileCabinetService = new FileCabinetMemoryService(Validator);
                    break;
            }
        }
    }
}