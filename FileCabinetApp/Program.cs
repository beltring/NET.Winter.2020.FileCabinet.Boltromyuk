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
        private static IFileCabinetService fileCabinetService;
        private static ValidatorType validatorType;
        private static ServiceType serviceType;
        private static IRecordValidator validator;
        private static bool isRunning;

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

            isRunning = true;
            ParseArguments(args);
            CreateService(validatorType, serviceType);
            var commandHandler = CreateCommandHandlers();

            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine($"Using {validatorType} validation rules.");
            Console.WriteLine($"The {serviceType} service.");
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
            while (isRunning);
        }

        private static void ParseArguments(string[] args)
        {
            bool isCustom = args.Any(x => x.Contains(ValidatorType.Custom.ToString(), StringComparison.OrdinalIgnoreCase));
            bool isFile = args.Any(x => x.Contains(ServiceType.File.ToString(), StringComparison.OrdinalIgnoreCase));

            validatorType = isCustom ? ValidatorType.Custom : ValidatorType.Default;
            serviceType = isFile ? ServiceType.File : ServiceType.Memory;
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var exitHandler = new ExitCommandHandler(fileStream, x => isRunning = x);
            var helpHandler = new HelpCommandHandler();
            var createHandle = new CreateCommandHandler(fileCabinetService, validator);
            var editHandler = new EditCommandHandler(fileCabinetService, validator);
            var findHandler = new FindCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var listHandle = new ListCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var purgeHadler = new PurgeCommandHandler(fileCabinetService);
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
                    validator = new CustomValidator();
                    break;
                case ValidatorType.Default:
                    validator = new DefaultValidator();
                    break;
            }

            switch (serviceType)
            {
                case ServiceType.File:
                    fileStream = new FileStream(DefaultBinaryFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fileCabinetService = new FileCabinetFilesystemService(validator, fileStream);
                    break;
                case ServiceType.Memory:
                    fileCabinetService = new FileCabinetMemoryService(validator);
                    break;
            }
        }
    }
}